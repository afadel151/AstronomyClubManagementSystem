using System.Security.Claims;

namespace Api.Auth;

public interface ICurrentUserService
{
    /// <summary>Strongly-typed user ID. Throws if not authenticated.</summary>
    Guid UserId { get; }

    /// <summary>Nullable variant — safe to call without an active auth context.</summary>
    Guid? UserIdOrNull { get; }

    string? MemberId { get; }
    string? Email { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
}

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public Guid? UserIdOrNull
    {
        get
        {
            var raw = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }

    public Guid UserId =>
        UserIdOrNull ?? throw new UnauthorizedAccessException("User is not authenticated.");

    public string? MemberId => Principal?.FindFirstValue("member_id");

    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyList<string> Roles =>
        Principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? [];

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;
}
