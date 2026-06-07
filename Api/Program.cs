using Api.Middleware;
using Application.Auth;
using Application.Helpers;
using Data.Context;
using Data.Entities.Identity;
using Data.MongoDB;
using Infrastructure.Helpers;
using Infrastructure.Microservices;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using MongoDB.Driver;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/astro-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.Configure<AuthOptions>(
    builder.Configuration.GetSection(AuthOptions.SectionName));

builder.Services.AddDbContext<AstroClubDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer"),
        sql => sql.MigrationsAssembly("Data")
    )
);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength         = 8;
    options.Password.RequireDigit           = false;
    options.Password.RequireUppercase       = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(2);
    options.User.RequireUniqueEmail         = true;
    options.SignIn.RequireConfirmedEmail     = false;
})
.AddEntityFrameworkStores<AstroClubDbContext>()
.AddDefaultTokenProviders();

// ── Authentication ────────────────────────────────────────────────────────────

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = AuthConstants.SmartScheme;
})
.AddPolicyScheme(AuthConstants.SmartScheme, "Cookie or JWT", options =>
{
    options.ForwardDefaultSelector = ctx =>
    {
        var auth = ctx.Request.Headers["Authorization"].FirstOrDefault();
        if (auth?.StartsWith("Bearer ") == true)
            return JwtBearerDefaults.AuthenticationScheme;

        return CookieAuthenticationDefaults.AuthenticationScheme;
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath         = "/account/login";
    options.AccessDeniedPath  = "/account/access-denied";
    options.ExpireTimeSpan    = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly   = true;
    options.Cookie.SameSite   = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var authOptions = builder.Configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>() ?? new AuthOptions();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = authOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };

    options.Events = new JwtBearerEvents
    {
        // Allow JWT from websocket query string (SignalR).
        OnMessageReceived = ctx =>
        {
            var token = ctx.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(token) &&
                ctx.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                ctx.Token = token;
            return Task.CompletedTask;
        },

        // Return JSON 401 instead of a redirect — Vue / mobile clients need this.
        OnChallenge = async ctx =>
        {
            ctx.HandleResponse();
            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ctx.Response.ContentType = "application/json";
            var body = JsonSerializer.Serialize(new
            {
                code = "unauthorized",
                message = ctx.ErrorDescription ?? "Authentication is required."
            });
            await ctx.Response.WriteAsync(body);
        },

        // Return JSON 403 instead of a redirect.
        OnForbidden = async ctx =>
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            ctx.Response.ContentType = "application/json";
            var body = JsonSerializer.Serialize(new
            {
                code = "forbidden",
                message = "You do not have permission to access this resource."
            });
            await ctx.Response.WriteAsync(body);
        }
    };
});

// ── Authorization ─────────────────────────────────────────────────────────────

builder.Services.AddAuthorization(options =>
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

// ── CORS ──────────────────────────────────────────────────────────────────────

// Allowed origins are read from config so they can be overridden per environment.
// appsettings.json key: "Cors:AllowedOrigins" (string array)
var corsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:5173", "http://localhost:3000"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("SpaAndMobile", policy =>
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());         // needed for cookie auth from Blazor
});


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


// ── Infrastructure ────────────────────────────────────────────────────────────

builder.Services.AddSingleton<IMongoClient>(_ =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));
builder.Services.AddSingleton<MongoDbContext>(sp =>
    new MongoDbContext(sp.GetRequiredService<IMongoClient>(), "AstroClubMongo"));

builder.Services.AddSingleton<IMinioClient>(_ =>
    new MinioClient()
        .WithEndpoint(builder.Configuration["MinIO:Endpoint"])
        .WithCredentials(
            builder.Configuration["MinIO:AccessKey"],
            builder.Configuration["MinIO:SecretKey"])
        .WithSSL(false)
        .Build());
builder.Services.AddScoped<IStorageService, MinioStorageService>();

// Configure Microservices options
builder.Services.Configure<Infrastructure.Microservices.MicroserviceOptions>(
    builder.Configuration.GetSection(Infrastructure.Microservices.MicroserviceOptions.SectionName));

// Register Typed Resilient HttpClient for the Example Client
builder.Services.AddMicroserviceClient<
    Infrastructure.Microservices.IExampleServiceClient,
    Infrastructure.Microservices.ExampleServiceClient>("Example");

// ── API ───────────────────────────────────────────────────────────────────────

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var mongoCtx = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await MongoIndexInitializer.InitializeAsync(mongoCtx);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SpaAndMobile");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
