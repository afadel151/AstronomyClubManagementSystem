using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;

namespace Web.Club.Components.Base;



public abstract class AuthorizedComponentBase : ComponentBase
{
    [Inject] protected NavigationManager Nav { get; set; } = default!;
    [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    private bool _dataLoaded = false;

    protected virtual Task OnPageInitializedAsync() => Task.CompletedTask;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || _dataLoaded) return;
        _dataLoaded = true;

        bool isAuthenticated = false;
        for (int i = 0; i < 3; i++)
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            isAuthenticated = authState.User.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated) break;
        }

        if (!isAuthenticated)
        {
            Nav.NavigateTo("/account/login", forceLoad: true);
            return;
        }
        try
        {
            await OnPageInitializedAsync();
            StateHasChanged();
        }
        catch (HttpRequestException ex) when ((int?)ex.StatusCode == 401)
        {
            Nav.NavigateTo("/account/login", forceLoad: true);
        }
        catch (HttpRequestException ex) when ((int?)ex.StatusCode == 403)
        {
            Nav.NavigateTo("/account/access-denied", forceLoad: true);
        }
    }
}