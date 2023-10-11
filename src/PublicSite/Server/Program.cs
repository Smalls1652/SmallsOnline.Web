using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.PublicSite.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddRazorPages();

builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>(
    provider => new CosmosDbService(
        connectionString: builder.Configuration.GetValue<string>("CosmosDbConnectionString")!,
        containerName: builder.Configuration.GetValue<string>("CosmosDbContainerName")!
    )
);

var app = builder.Build();

app.UseAntiforgery();

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
    .AddInteractiveServerRenderMode();

app.MapRazorPages();
//app.MapFallbackToPage("/error");
app.UseStatusCodePagesWithReExecute("/error/{0}");

await app.RunAsync();