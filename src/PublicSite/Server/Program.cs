using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Hosting;
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
    .AddWebAssemblyComponents()
    .AddServerComponents();

builder.Services
    .AddRazorPages();

builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>(
    provider => new CosmosDbService(
        connectionString: builder.Configuration.GetValue<string>("CosmosDbConnectionString")!,
        containerName: builder.Configuration.GetValue<string>("CosmosDbContainerName")!
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app
    .MapRazorComponents<App>()
    .AddWebAssemblyRenderMode()
    .AddServerRenderMode();

app.MapRazorPages();

app.Run();
