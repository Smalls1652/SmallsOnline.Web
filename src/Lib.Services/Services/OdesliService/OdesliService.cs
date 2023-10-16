using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Reflection;
using SmallsOnline.Web.Lib.Models.Odesli;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Service for interacting with the Odesli API.
/// </summary>
public partial class OdesliService : IOdesliService
{
    /// <summary>
    /// Logger for the service.
    /// </summary>
    private readonly ILogger<OdesliService> _logger;

    /// <summary>
    /// <see cref="IHttpClientFactory"/> for the service.
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="OdesliService"/> class.
    /// </summary>
    /// <param name="logger">Logger for the service.</param>
    /// <param name="httpClientFactory">HTTP client factory for the service.</param>
    public OdesliService(ILogger<OdesliService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public async Task<MusicEntityItem?> GetShareLinksAsync(string inputUrl)
    {
        var httpClient = _httpClientFactory.CreateClient("OdesliApiClient");

        _logger.LogInformation("Getting share links for '{inputUrl}'.", inputUrl);
        string encodedUrl = WebUtility.UrlEncode(inputUrl);

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"links?url={encodedUrl}"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        await using var contentStream = await responseMessage.Content.ReadAsStreamAsync();
        MusicEntityItem? musicEntityItem = await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: OdesliJsonContext.Default.MusicEntityItem
        );

        return musicEntityItem;
    }
}