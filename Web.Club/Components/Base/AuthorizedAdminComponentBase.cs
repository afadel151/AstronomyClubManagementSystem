using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Web.Club.Auth;

namespace Web.Club.Components.Base;



public abstract class AuthorizedAdminComponentBase : AuthorizedComponentBase
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            Nav.NavigateTo("/account/login", forceLoad: true);
            return;
        }

        if (!user.IsInRole(AuthRoles.Admin))
        {
            Nav.NavigateTo("/account/access-denied", forceLoad: true);
            return;
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}
