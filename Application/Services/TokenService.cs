using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Auth;
using Data.Entities;
using Data.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public interface ITokenService
{
    (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(ApplicationUser user, IList<string> roles);
    (string RawToken, RefreshToken RefreshToken) GenerateRefreshToken(ApplicationUser user, string? ipAddress);
    ClaimsPrincipal ValidateExpiredToken(string token);
    string HashRefreshToken(string token);
}

public sealed class TokenService(IOptions<AuthOptions> options) : ITokenService
{
    private readonly AuthOptions _options = options.Value;

    public (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(ApplicationUser user, IList<string> roles)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_options.AccessTokenExpiryMinutes);
        var credentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256);
        var firstName = GetFirstName(user);
        var lastName = GetLastName(user);

        // Canonical user-id: ClaimTypes.NameIdentifier only.
        // Standard JWT email claim kept for interop; no duplicate "sub".
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(ClaimTypes.Name, user.FullName),
            new(JwtRegisteredClaimNames.GivenName, firstName),
            new(ClaimTypes.GivenName, firstName),
            new(JwtRegisteredClaimNames.FamilyName, lastName),
            new(ClaimTypes.Surname, lastName),
            new("member_id", user.MemberCode),
            new("is_active", IsActive(user).ToString().ToLowerInvariant())
        ];

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public (string RawToken, RefreshToken RefreshToken) GenerateRefreshToken(ApplicationUser user, string? ipAddress)
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = HashRefreshToken(rawToken),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenExpiryDays),
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByIp = ipAddress
        };

        return (rawToken, refreshToken);
    }

    public ClaimsPrincipal ValidateExpiredToken(string token)
    {
        var validationParameters = CreateValidationParameters();
        validationParameters.ValidateLifetime = false;

        return new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);
    }

    public string HashRefreshToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }

    private TokenValidationParameters CreateValidationParameters() => new()
    {
        ValidateIssuer = true,
        ValidIssuer = _options.Issuer,
        ValidateAudience = true,
        ValidAudience = _options.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = GetSecurityKey(),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };

    private SymmetricSecurityKey GetSecurityKey() =>
        new(Encoding.UTF8.GetBytes(_options.Key));

    private static string GetFirstName(ApplicationUser user)
    {
        var parts = user.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.FirstOrDefault() ?? user.DisplayName ?? string.Empty;
    }

    private static string GetLastName(ApplicationUser user)
    {
        var parts = user.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : string.Empty;
    }

    private static bool IsActive(ApplicationUser user) =>
        user.MemberStatus is not (Data.Entities.Enums.MemberStatusEnum.Inactive or Data.Entities.Enums.MemberStatusEnum.Suspended);
}
