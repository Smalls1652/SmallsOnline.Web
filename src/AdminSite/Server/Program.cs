using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.AdminSite.Server;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddServerComponents();

builder.Services
    .AddRazorPages();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app
    .MapRazorComponents<App>()
    .AddServerRenderMode();

app.MapRazorPages();
//app.MapFallbackToPage("/error");
app.UseStatusCodePagesWithReExecute("/error/{0}");

await app.RunAsync();