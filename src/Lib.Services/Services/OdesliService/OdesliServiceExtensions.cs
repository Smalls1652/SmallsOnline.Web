using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace SmallsOnline.Web.Lib.Services;

public static class OdesliServiceExtensions
{
    public static IServiceCollection AddOdesliService(this IServiceCollection services, string userAgentName)
    {
        services.AddHttpClient(
            name: "OdesliApiClient",
            configureClient: (serviceProvider, httpClient) =>
            {
                httpClient.DefaultRequestHeaders.UserAgent.Add(new(userAgentName, Assembly.GetExecutingAssembly().GetName().Version!.ToString()));
                httpClient.BaseAddress = new("https://api.song.link/v1-alpha.1/");
            }
        );

        services.AddSingleton<IOdesliService, OdesliService>();

        return services;
    }

    public static IServiceCollection AddItunesApiService(this IServiceCollection services, string userAgentName, Action<OdesliServiceOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient(
            name: "OdesliApiClient",
            configureClient: (serviceProvider, httpClient) =>
            {
                httpClient.DefaultRequestHeaders.UserAgent.Add(new(userAgentName, Assembly.GetExecutingAssembly().GetName().Version!.ToString()));
                httpClient.BaseAddress = new("https://api.song.link/v1-alpha.1/");
            }
        );

        services.AddSingleton<IOdesliService, OdesliService>();

        return services;
    }
}