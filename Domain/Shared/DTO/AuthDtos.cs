using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Shared.DTO;

// ── Enums ─────────────────────────────────────────────────────────────────────

/// <summary>
/// Tells the server what kind of client is authenticating.
/// <list type="bullet">
///   <item><term>Browser</term><description>Blazor / MVC — issue cookie + JWT.</description></item>
///   <item><term>Api</term><description>Vue SPA, mobile app, Python script — JWT only, no cookie.</description></item>
/// </list>
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AuthClientType
{
    Browser,
    Api
}

// ── Login ─────────────────────────────────────────────────────────────────────

public sealed record LoginRequest(
    string Email,
    string Password,
    bool RememberMe,
    AuthClientType ClientType = AuthClientType.Api);

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    UserDto User)
{
    [JsonIgnore]
    public string RefreshToken { get; init; } = string.Empty;

    /// <summary>True when the server issued an Identity cookie alongside the JWT.</summary>
    public bool CookieIssued { get; init; }
}

// ── Refresh ───────────────────────────────────────────────────────────────────

public sealed record RefreshResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt)
{
    [JsonIgnore]
    public string RefreshToken { get; init; } = string.Empty;
}

// ── User ──────────────────────────────────────────────────────────────────────

public sealed record UserDto(
    string Id,
    string MemberId,
    string Email,
    string FirstName,
    string LastName,
    IList<string> Roles);

// ── Register ──────────────────────────────────────────────────────────────────

/// <summary>
/// Payload for POST /api/auth/register.
/// Validation is enforced by DataAnnotations and double-checked in AuthService.
/// </summary>
public sealed record RegisterRequest
{
    /// <summary>Full legal name — used as FullName and split into first/last for claims.</summary>
    [Required, MaxLength(200)]
    public string FullName { get; init; } = string.Empty;

    /// <summary>Unique email address — becomes UserName too (Identity convention).</summary>
    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    /// <summary>Password — minimum 8 characters (aligned with Identity options in Program.cs).</summary>
    [Required, MinLength(8), MaxLength(100)]
    public string Password { get; init; } = string.Empty;

    /// <summary>Password confirmation — validated in the service layer against Password.</summary>
    [Required]
    public string ConfirmPassword { get; init; } = string.Empty;

    /// <summary>Client type — determines whether an Identity cookie is issued.</summary>
    public AuthClientType ClientType { get; init; } = AuthClientType.Api;

    // ── Optional profile fields (captured at signup) ──────────────────────

    [MaxLength(100)]
    public string? DisplayName { get; init; }

    [MaxLength(100)]
    public string? Nationality { get; init; }

    [MaxLength(100)]
    public string? City { get; init; }

    /// <summary>AAVSO observer code — optional; advanced members only.</summary>
    [MaxLength(50)]
    public string? AavsoObserverCode { get; init; }
}

/// <summary>
/// Returned on successful registration.
/// Access token is issued immediately so the user is logged in right away.
/// Refresh token goes in an HttpOnly cookie (same as login).
/// </summary>
public sealed record RegisterResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    UserDto User)
{
    [JsonIgnore]
    public string RefreshToken { get; init; } = string.Empty;

    /// <summary>True when the server issued an Identity cookie alongside the JWT.</summary>
    public bool CookieIssued { get; init; }
}