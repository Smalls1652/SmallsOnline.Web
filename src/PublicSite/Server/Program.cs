using Microsoft.Extensions.Http;
using SmallsOnline.Web.PublicSite.Server;
using SmallsOnline.Web.PublicSite.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddSingleton<FavoritesOfStateContainer>();

// Add scoped HTTP client.
builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri(builder.Environment.WebRootPath) }
);

// Add HTTP client service for accessing the base website.
builder.Services.AddHttpClient(
    name: "BaseAppClient",
    configureClient: (client) => { client.BaseAddress = new(builder.Environment.WebRootPath); }
);

// Attempt to get the API URL from the config.
string? apiUri = builder.Configuration.GetValue<string>("ApiUri");

// If the API URL is null, throw and exception..
if (apiUri is null)
{
    throw new InvalidOperationException("The API URI was not found in the configuration.");
}

// Add HTTP client service for accessing the API.
builder.Services.AddHttpClient(
    name: "PublicApi",
    configureClient: (client) => { client.BaseAddress = new(apiUri); }
);

builder.Services.Remove(builder.Services.First(s => s.ServiceType == typeof(IHttpMessageHandlerBuilderFilter)));

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

app.MapRazorComponents<App>();

app.Run();
