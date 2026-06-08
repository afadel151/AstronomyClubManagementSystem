using Domain.Shared.Schemas;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Radzen;
using Web.Club.Auth;
using Web.Club.Components;
using Web.Club.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Blazor ────────────────────────────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── Auth config ───────────────────────────────────────────────────────────────
builder.Services.Configure<AuthApiOptions>(
    builder.Configuration.GetSection(AuthApiOptions.SectionName));
builder.Services
    .AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/forbidden";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthConstants.Policies.ManageEvents, p =>
        p.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin, AuthConstants.Roles.EventManager));

    options.AddPolicy(AuthConstants.Policies.ManageInventory, p =>
        p.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin, AuthConstants.Roles.InventoryManager));

    options.AddPolicy(AuthConstants.Policies.ManageMembers, p =>
        p.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin, AuthConstants.Roles.BoardMember));

    options.AddPolicy(AuthConstants.Policies.ManageUsers, p =>
        p.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.SuperAdmin));
});

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthApiOptions>();
builder.Services.AddScoped<CircuitTokenStore>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddHttpClient("Api", (sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<AuthApiOptions>>().Value;
    client.BaseAddress = new Uri(opts.BaseUrl);
})
.AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddScoped<ApiCookieAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ApiCookieAuthenticationStateProvider>());

// ── Other services ─────────────────────────────────────────────────────────────
builder.Services.AddRadzenComponents();

var app = builder.Build();

// ── Pipeline ───────────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// BFF cookie endpoints — the only place cookies are written
app.MapBffEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();