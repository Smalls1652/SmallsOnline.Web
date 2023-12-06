using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.AdminSite.Server;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

builder.Services
    .AddHealthChecks();

builder.Services
    .AddCascadingAuthenticationState();

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("EntraAppId")!;
        options.ClientSecret = builder.Configuration.GetValue<string>("EntraAppClientSecret")!;
        options.TenantId = builder.Configuration.GetValue<string>("EntraTenantId")!;
        options.Instance = "https://login.microsoftonline.com/";
        options.CallbackPath = "/signin-oidc";
    });

builder.Services
    .AddAuthorization(
        options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;
        }
    );

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services
    .AddMicrosoftIdentityConsentHandler();

builder.Services
    .AddHttpClient(
        name: "GenericClient"
    );

builder.Services
    .AddCosmosDbService(
        options =>
        {
            options.ConnectionString = builder.Configuration.GetValue<string>("CosmosDbConnectionString")!;
            options.ContainerName = builder.Configuration.GetValue<string>("CosmosDbContainerName")!;
        }
    )
    .AddBlobStorageService(
        options =>
        {
            options.ConnectionString = builder.Configuration.GetValue<string>("StorageAccountConnectionString")!;
            options.StorageAccountDomainName = builder.Configuration.GetValue<string>("StorageAccountDomainName")!;
        }
    )
    .AddItunesApiService("SmallsOnline.Web.AdminSite.Server")
    .AddOdesliService("SmallsOnline.Web.AdminSite.Server");

builder.Services.Configure<ForwardedHeadersOptions>(
    options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    }
);

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseAntiforgery();
app.MapControllers();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHealthChecks("/healthz");

await app.RunAsync();