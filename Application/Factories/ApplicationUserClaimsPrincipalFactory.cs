using System.Security.Claims;
using Data.Entities.Enums;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Factories;

public sealed class ApplicationUserClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor)
    : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var (firstName, lastName) = SplitFullName(user);

        // Canonical user-id: ClaimTypes.NameIdentifier only (matches JWT config).
        AddOrReplace(identity, ClaimTypes.NameIdentifier, user.Id);
        AddOrReplace(identity, ClaimTypes.Email, user.Email ?? string.Empty);
        AddOrReplace(identity, ClaimTypes.Name, user.FullName);
        AddOrReplace(identity, ClaimTypes.GivenName, firstName);
        AddOrReplace(identity, ClaimTypes.Surname, lastName);
        AddOrReplace(identity, "member_id", user.MemberCode);
        AddOrReplace(identity, "is_active", IsActive(user).ToString().ToLowerInvariant());

        return identity;
    }

    private static void AddOrReplace(ClaimsIdentity identity, string type, string value)
    {
        foreach (var claim in identity.FindAll(type).ToList())
        {
            identity.RemoveClaim(claim);
        }

        identity.AddClaim(new Claim(type, value));
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

    private static bool IsActive(ApplicationUser user) =>
        user.MemberStatus is not (MemberStatusEnum.Inactive or MemberStatusEnum.Suspended);
}
