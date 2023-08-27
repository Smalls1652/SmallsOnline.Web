using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Http;
using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.PublicSite.Server;
using SmallsOnline.Web.PublicSite.Server.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddServerComponents()
    .AddWebAssemblyComponents();


builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>(
    provider => new CosmosDbService(
        connectionString: builder.Configuration.GetValue<string>("CosmosDbConnectionString")!,
        containerName: builder.Configuration.GetValue<string>("CosmosDbContainerName")!
    )
);

// Temporarily enable SynchronousIO to fix issues with certain DB calls.
builder.Services
    .Configure<KestrelServerOptions>(options =>
    {
        options.AllowSynchronousIO = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app
    .MapRazorComponents<App>()
    .AddServerRenderMode()
    .AddWebAssemblyRenderMode();

app.Run();
