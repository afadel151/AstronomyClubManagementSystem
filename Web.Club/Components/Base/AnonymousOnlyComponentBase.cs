using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace Web.Club.Components.Base;

public abstract class AnonymousOnlyComponentBase : ComponentBase
{
    [Inject] protected NavigationManager Nav { get; set; } = default!;
    [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    [Inject] protected NotificationService NotificationService {get;set;} = default!;

    [Parameter] public string? ReturnUrl { get; set; }
    [Parameter] public string? Error { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();

        if (authState.User.Identity?.IsAuthenticated == true)
        {
            Nav.NavigateTo(ReturnUrl ?? "/", forceLoad: true);
        }
    }
}