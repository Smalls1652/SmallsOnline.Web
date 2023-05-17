using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Http;

// Create WebAssemblyHostBuilder
WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

// Add service for maintaining the state of the Favorite Of data.
builder.Services.AddSingleton<FavoritesOfStateContainer>();

// Add scoped HTTP client.
builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

// Add HTTP client service for accessing the base website.
builder.Services.AddHttpClient(
    name: "BaseAppClient",
    configureClient: (client) => { client.BaseAddress = new(builder.HostEnvironment.BaseAddress); }
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

// Remove HttpClient messages from being generated.
builder.Services.Remove(builder.Services.First(s => s.ServiceType == typeof(IHttpMessageHandlerBuilderFilter)));

// Build the host.
var app = builder.Build();

// Run the app.
await app.RunAsync();