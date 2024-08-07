using System.Net;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.PublicSite.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

builder.Services
    .AddHealthChecks();

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services
    .AddRazorPages();

builder.Services.AddCosmosDbService(
    options =>
    {
        options.ConnectionString = builder.Configuration.GetValue<string>("CosmosDbConnectionString")!;
        options.ContainerName = builder.Configuration.GetValue<string>("CosmosDbContainerName")!;
    }
);

builder.Services.Configure<ForwardedHeadersOptions>(
    options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    }
);

string? dataProtectionPath = builder.Configuration.GetValue<string>("DataProtectionKeysPath");
if (dataProtectionPath is not null || !string.IsNullOrEmpty(dataProtectionPath))
{
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath));
}

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseForwardedHeaders();

app.UseAntiforgery();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app
    .MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode();

app.MapRazorPages();
//app.MapFallbackToPage("/error");
app.UseStatusCodePagesWithReExecute("/error/{0}");

app
    .MapHealthChecks("/healthz");

await app.RunAsync();