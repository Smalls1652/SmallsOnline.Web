using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.AdminSite.Server;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

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
    .AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services
    .AddAuthorization();

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services
    .AddMicrosoftIdentityConsentHandler();

builder.Services
    .AddHttpClient(
        name: "GenericClient"
    );

builder.Services
    .AddHttpClient(
        name: "OdesliApiClient",
        configureClient: (serviceProvider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.UserAgent.Add(new("SmallsOnline.Web.AdminSite.Server", Assembly.GetExecutingAssembly().GetName().Version!.ToString()));
            httpClient.BaseAddress = new("https://api.song.link/v1-alpha.1/");
        }
    );

builder.Services
    .AddHttpClient(
        name: "ItunesApiClient",
        configureClient: (serviceProvider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.UserAgent.Add(new("MuzakBot", Assembly.GetExecutingAssembly().GetName().Version!.ToString()));
            httpClient.BaseAddress = new("https://itunes.apple.com/");
        }
    );

builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>(
    provider => new CosmosDbService(
        connectionString: builder.Configuration.GetValue<string>("CosmosDbConnectionString")!,
        containerName: builder.Configuration.GetValue<string>("CosmosDbContainerName")!
    )
);

builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>(
    provider => new BlobStorageService(
        connectionString: builder.Configuration.GetValue<string>("StorageAccountConnectionString")!,
        storageAccountDomainName: builder.Configuration.GetValue<string>("StorageAccountDomainName")!
    )
);

builder.Services.AddSingleton<IOdesliService, OdesliService>();
builder.Services.AddSingleton<IItunesApiService, ItunesApiService>();

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

await app.RunAsync();