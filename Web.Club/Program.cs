using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Club.Auth;
using Web.Club.Components;
using Domain.Shared.Schemas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<AuthApiOptions>(
    builder.Configuration.GetSection(AuthApiOptions.SectionName));
    
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(AuthConstants.Policies.ManageEvents, policy =>
        policy.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin, AuthConstants.Roles.EventManager));
    options.AddPolicy(AuthConstants.Policies.ManageInventory, policy =>
        policy.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin, AuthConstants.Roles.InventoryManager));
    options.AddPolicy(AuthConstants.Policies.ManageMembers, policy =>
        policy.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin, AuthConstants.Roles.BoardMember));
    options.AddPolicy(AuthConstants.Policies.ManageUsers, policy =>
        policy.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin));
});
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient<Web.Club.Services.AuthenticationService>((sp, client) =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<AuthApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = new System.Net.CookieContainer()
});
builder.Services.AddScoped<ApiCookieAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ApiCookieAuthenticationStateProvider>());

builder.Services.AddRadzenComponents();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
