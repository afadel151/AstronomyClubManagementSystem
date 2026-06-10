using System.ComponentModel.DataAnnotations;
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
using RefreshTokenEntity = Data.Entities.Generated.RefreshToken;

namespace Application.Services;

public interface IAuthService
{
    Task<LoginResponse>   LoginAsync(LoginRequest request, string? ipAddress, CancellationToken ct);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, string? ipAddress, CancellationToken ct);
    Task<RefreshResponse> RefreshAsync(string? refreshToken, string? ipAddress, CancellationToken ct);
    Task                  LogoutAsync(string? refreshToken, string? ipAddress, CancellationToken ct);
    UserDto               GetCurrentUserFromClaims(ClaimsPrincipal principal);
}

public sealed class AuthService(
    UserManager<ApplicationUser>          userManager,
    IBaseRepository<ApplicationUser>      userRepository,
    IBaseRepository<RefreshTokenEntity>   refreshTokenRepository,
    ITokenService                         tokenService) : IAuthService
{
    // ── Login ─────────────────────────────────────────────────────────────────

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request, string? ipAddress, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (user.MemberStatus is MemberStatusEnum.Inactive or MemberStatusEnum.Suspended)
            throw new AccountDisabledException("User account is disabled.");

        var roles        = await userManager.GetRolesAsync(user);
        var accessToken  = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken(user, ipAddress);

        await refreshTokenRepository.AddAsync(
            refreshToken.RefreshToken, saveChanges: false, cancellationToken: ct);

        user.LastLoginAt = DateTimeOffset.UtcNow;
        user.LastLoginIp = ipAddress;
        await userManager.UpdateAsync(user);
        await refreshTokenRepository.SaveChangesAsync(ct);

        return new LoginResponse(accessToken.Token, accessToken.ExpiresAt, CreateUserDto(user, roles))
        {
            RefreshToken = refreshToken.RawToken
        };
    }

    // ── Register ──────────────────────────────────────────────────────────────

    public async Task<RegisterResponse> RegisterAsync(
        RegisterRequest request, string? ipAddress, CancellationToken ct)
    {
        if (request.Password != request.ConfirmPassword)
            throw new ValidationException("Password and confirmation do not match.");

        if (await userManager.FindByEmailAsync(request.Email) is not null)
            throw new ValidationException("An account with this email already exists.");

        var user = new ApplicationUser
        {
            UserName          = request.Email,
            Email             = request.Email,
            FullName          = request.FullName.Trim(),
            DisplayName       = request.DisplayName?.Trim(),
            MemberCode        = await GenerateMemberCodeAsync(ct),
            MemberStatus      = MemberStatusEnum.Pending,
            Nationality       = request.Nationality?.Trim(),
            City              = request.City?.Trim(),
            AavsoObserverCode = request.AavsoObserverCode?.Trim(),
            JoinDate          = DateOnly.FromDateTime(DateTime.UtcNow),
            CreatedAt         = DateTimeOffset.UtcNow,
            UpdatedAt         = DateTimeOffset.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new ValidationException(errors);
        }

        await userManager.AddToRoleAsync(user, AuthConstants.Roles.Member);
        var roles        = await userManager.GetRolesAsync(user);
        var accessToken  = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken(user, ipAddress);

        await refreshTokenRepository.AddAsync(
            refreshToken.RefreshToken, saveChanges: false, cancellationToken: ct);

        user.LastLoginAt = DateTimeOffset.UtcNow;
        user.LastLoginIp = ipAddress;
        await userManager.UpdateAsync(user);
        await refreshTokenRepository.SaveChangesAsync(ct);

        return new RegisterResponse(accessToken.Token, accessToken.ExpiresAt, CreateUserDto(user, roles))
        {
            RefreshToken = refreshToken.RawToken
        };
    }


    public async Task<RefreshResponse> RefreshAsync(
        string? refreshToken, string? ipAddress, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new UnauthorizedAccessException("Refresh token is missing.");

        var hashedToken  = tokenService.HashRefreshToken(refreshToken);
        var storedToken  = await refreshTokenRepository.SingleOrDefaultAsync(
            t     => t.Token == hashedToken,
            query => query.Include(t => t.User),
            asNoTracking: false,
            cancellationToken: ct);

        if (storedToken is null
            || storedToken.RevokedAt is not null
            || storedToken.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var user = storedToken.User;

        if (user.MemberStatus is MemberStatusEnum.Inactive or MemberStatusEnum.Suspended)
            throw new AccountDisabledException("User account is disabled.");

        var roles        = await userManager.GetRolesAsync(user);
        var accessToken  = tokenService.GenerateAccessToken(user, roles);
        var replacement  = tokenService.GenerateRefreshToken(user, ipAddress);

        // Rotate: revoke old, persist new
        storedToken.RevokedAt       = DateTimeOffset.UtcNow;
        storedToken.RevokedByIp     = ipAddress;
        storedToken.ReplacedByToken = replacement.RefreshToken.Token;

        await refreshTokenRepository.AddAsync(
            replacement.RefreshToken, saveChanges: false, cancellationToken: ct);
        await refreshTokenRepository.SaveChangesAsync(ct);

        return new RefreshResponse(accessToken.Token, accessToken.ExpiresAt)
        {
            RefreshToken = replacement.RawToken
        };
    }

    // ── Logout ────────────────────────────────────────────────────────────────

    public async Task LogoutAsync(
        string? refreshToken, string? ipAddress, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        var hashedToken = tokenService.HashRefreshToken(refreshToken);
        var storedToken = await refreshTokenRepository.SingleOrDefaultAsync(
            t => t.Token == hashedToken,
            asNoTracking: false,
            cancellationToken: ct);

        if (storedToken is not null && storedToken.RevokedAt is null)
        {
            storedToken.RevokedAt   = DateTimeOffset.UtcNow;
            storedToken.RevokedByIp = ipAddress;
            await refreshTokenRepository.SaveChangesAsync(ct);
        }
    }

    // ── Current user from claims (no DB hit) ──────────────────────────────────

    public UserDto GetCurrentUserFromClaims(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        return new UserDto(
            userId,
            MemberId:  principal.FindFirstValue("member_id")        ?? string.Empty,
            Email:     principal.FindFirstValue(ClaimTypes.Email)    ?? string.Empty,
            FirstName: principal.FindFirstValue(ClaimTypes.GivenName)  ?? string.Empty,
            LastName:  principal.FindFirstValue(ClaimTypes.Surname)    ?? string.Empty,
            Roles:     principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList());
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
        var parts = user.FullName.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        return parts.Length switch
        {
            0 => (user.DisplayName ?? string.Empty, string.Empty),
            1 => (parts[0], string.Empty),
            _ => (parts[0], parts[1])
        };
    }

    private async Task<string> GenerateMemberCodeAsync(CancellationToken ct)
    {
        var prefix = $"ASTRO-{DateTime.UtcNow:yyyyMM}-";
        string code;

        do
        {
            code = prefix + Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
        }
        while (await userRepository.AnyAsync(u => u.MemberCode == code, ct));

        return code;
    }
}