using System.Security.Claims;
using Application.Auth;
using Application.Repositories;
using Data.Entities.Enums;
using Data.Entities.Identity;
using Domain.Shared.DTO;
using Domain.Shared.Exceptions;
using Domain.Shared.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RefreshTokenEntity = Data.Entities.RefreshToken;

namespace Application.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress, CancellationToken ct);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, string? ipAddress, CancellationToken ct);
    Task<RefreshResponse> RefreshAsync(string? refreshToken, string? ipAddress, CancellationToken ct);
    System.Threading.Tasks.Task LogoutAsync(string? refreshToken, string? ipAddress, CancellationToken ct);
    UserDto GetCurrentUserFromClaims(ClaimsPrincipal principal);
}

public sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IBaseRepository<ApplicationUser> userRepository,
    IBaseRepository<RefreshTokenEntity> refreshTokenRepository,
    ITokenService tokenService) : IAuthService
{
    // ── Login ─────────────────────────────────────────────────────────────────

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (user.MemberStatus is MemberStatusEnum.Inactive or MemberStatusEnum.Suspended)
        {
            throw new AccountDisabledException("User account is disabled.");
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken(user, ipAddress);

        await refreshTokenRepository.AddAsync(refreshToken.RefreshToken, saveChanges: false, cancellationToken: ct);
        user.LastLoginAt = DateTimeOffset.UtcNow;
        user.LastLoginIp = ipAddress;
        await userManager.UpdateAsync(user);
        await refreshTokenRepository.SaveChangesAsync(ct);

        // Only issue an Identity cookie for browser clients (Blazor, MVC).
        var issueCookie = request.ClientType == AuthClientType.Browser;
        if (issueCookie)
        {
            await signInManager.SignInAsync(user, request.RememberMe);
        }

        return new LoginResponse(
            accessToken.Token,
            accessToken.ExpiresAt,
            CreateUserDto(user, roles))
        {
            RefreshToken = refreshToken.RawToken,
            CookieIssued = issueCookie
        };
    }

    // ── Register ──────────────────────────────────────────────────────────────

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, string? ipAddress, CancellationToken ct)
    {
        // Validate password confirmation
        if (request.Password != request.ConfirmPassword)
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException(
                "Password and confirmation do not match.");
        }

        // Check for duplicate email
        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
        {
            throw new System.ComponentModel.DataAnnotations.ValidationException(
                "An account with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName.Trim(),
            DisplayName = request.DisplayName?.Trim(),
            MemberCode = await GenerateMemberCodeAsync(ct),
            MemberStatus = MemberStatusEnum.Pending,
            Nationality = request.Nationality?.Trim(),
            City = request.City?.Trim(),
            AavsoObserverCode = request.AavsoObserverCode?.Trim(),
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new System.ComponentModel.DataAnnotations.ValidationException(errors);
        }

        await userManager.AddToRoleAsync(user, AuthConstants.Roles.Member);
        var roles = await userManager.GetRolesAsync(user);

        var (Token, ExpiresAt) = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken(user, ipAddress);
        await refreshTokenRepository.AddAsync(refreshToken.RefreshToken, saveChanges: false, cancellationToken: ct);
        user.LastLoginAt = DateTimeOffset.UtcNow;
        user.LastLoginIp = ipAddress;
        await refreshTokenRepository.SaveChangesAsync(ct);

        // Only issue an Identity cookie for browser clients.
        var issueCookie = request.ClientType == AuthClientType.Browser;
        if (issueCookie)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
        }

        return new RegisterResponse(
            Token,
            ExpiresAt,
            CreateUserDto(user, roles))
        {
            RefreshToken = refreshToken.RawToken,
            CookieIssued = issueCookie
        };
    }

    // ── Refresh ───────────────────────────────────────────────────────────────

    public async Task<RefreshResponse> RefreshAsync(string? refreshToken, string? ipAddress, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new UnauthorizedAccessException("Refresh token is missing.");
        }

        var hashedToken = tokenService.HashRefreshToken(refreshToken);
        var storedToken = await refreshTokenRepository.SingleOrDefaultAsync(
            token => token.Token == hashedToken,
            query => query.Include(token => token.User),
            asNoTracking: false,
            cancellationToken: ct);

        if (storedToken is null || storedToken.RevokedAt is not null || storedToken.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token is invalid.");
        }

        var user = storedToken.User;
        if (user.MemberStatus is MemberStatusEnum.Inactive or MemberStatusEnum.Suspended)
        {
            throw new AccountDisabledException("User account is disabled.");
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var replacement = tokenService.GenerateRefreshToken(user, ipAddress);

        storedToken.RevokedAt = DateTimeOffset.UtcNow;
        storedToken.RevokedByIp = ipAddress;
        storedToken.ReplacedByToken = replacement.RefreshToken.Token;
        await refreshTokenRepository.AddAsync(replacement.RefreshToken, saveChanges: false, cancellationToken: ct);
        await refreshTokenRepository.SaveChangesAsync(ct);

        return new RefreshResponse(accessToken.Token, accessToken.ExpiresAt)
        {
            RefreshToken = replacement.RawToken
        };
    }

    // ── Logout ────────────────────────────────────────────────────────────────

    public async System.Threading.Tasks.Task LogoutAsync(string? refreshToken, string? ipAddress, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            var hashedToken = tokenService.HashRefreshToken(refreshToken);
            var storedToken = await refreshTokenRepository.SingleOrDefaultAsync(
                token => token.Token == hashedToken,
                asNoTracking: false,
                cancellationToken: ct);

            if (storedToken is not null && storedToken.RevokedAt is null)
            {
                storedToken.RevokedAt = DateTimeOffset.UtcNow;
                storedToken.RevokedByIp = ipAddress;
                await refreshTokenRepository.SaveChangesAsync(ct);
            }
        }

        await signInManager.SignOutAsync();
    }

    // ── Current User (claims-based — no DB hit) ──────────────────────────────

    public UserDto GetCurrentUserFromClaims(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Current user was not found.");

        var email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var firstName = principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
        var lastName = principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;
        var memberId = principal.FindFirstValue("member_id") ?? string.Empty;
        var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return new UserDto(userId, memberId, email, firstName, lastName, roles);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static UserDto CreateUserDto(ApplicationUser user, IList<string> roles)
    {
        var (firstName, lastName) = SplitFullName(user);

        return new UserDto(
            user.Id,
            user.MemberCode,
            user.Email ?? string.Empty,
            firstName,
            lastName,
            roles);
    }

    private static (string FirstName, string LastName) SplitFullName(ApplicationUser user)
    {
        var fullName = user.FullName.Trim();
        if (string.IsNullOrEmpty(fullName))
        {
            return (user.DisplayName ?? string.Empty, string.Empty);
        }

        var parts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 1 ? (parts[0], string.Empty) : (parts[0], parts[1]);
    }

    /// <summary>
    /// Generates a unique member code in the format ASTRO-yyyyMM-XXXX.
    /// </summary>
    private async Task<string> GenerateMemberCodeAsync(CancellationToken ct)
    {
        var prefix = $"ASTRO-{DateTime.UtcNow:yyyyMM}-";
        string code;

        do
        {
            code = prefix + Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
        }
        while (await userRepository.AnyAsync(user => user.MemberCode == code, ct));

        return code;
    }
}
