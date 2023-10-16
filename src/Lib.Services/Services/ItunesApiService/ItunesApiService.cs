using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SmallsOnline.Web.Lib.Models.Itunes;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Service for interacting with the iTunes Search API.
/// </summary>
public partial class ItunesApiService : IItunesApiService
{
    /// <summary>
    /// Logger for the service.
    /// </summary>
    private readonly ILogger<ItunesApiService> _logger;

    /// <summary>
    /// <see cref="IHttpClientFactory"/> for the service.
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItunesApiService"/> class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="httpClientFactory">The HTTP client factory for the service.</param>
    public ItunesApiService(ILogger<ItunesApiService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<ArtistItem>?> GetArtistSearchResultAsync(string artistName)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        string encodedSearch = WebUtility.UrlEncode(artistName);

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"search?country=US&media=music&entity=musicArtist&attribute=artistTerm&limit=5&term={encodedSearch}"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultArtistItem
        );
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<ArtistItem>?> GetArtistIdLookupResultAsync(string artistId)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"lookup?id={artistId}&entity=musicArtist"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultArtistItem
        );
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<SongItem>?> GetSongIdLookupResultAsync(string trackId)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"lookup?id={trackId}&entity=song"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultSongItem
        );
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<AlbumItem>?> GetAlbumIdLookupResultAsync(string albumId)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"lookup?id={albumId}&entity=album"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultAlbumItem
        );
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<SongItem>?> GetAlbumIdLookupSongsResultAsync(string albumId)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"lookup?id={albumId}&entity=song"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultSongItem
        );
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<SongItem>?> GetSongsByArtistResultAsync(string artistName, string songName)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        string encodedSearch = WebUtility.UrlEncode($"{artistName} {songName}");

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"search?country=US&media=music&entity=song&limit=5&term={encodedSearch}"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultSongItem
        );
    }

    /// <inheritdoc />
    public async Task<ApiSearchResult<AlbumItem>?> GetAlbumsByArtistResultAsync(string artistName, string albumName)
    {
        var httpClient = _httpClientFactory.CreateClient("ItunesApiClient");

        string encodedSearch = WebUtility.UrlEncode($"{artistName} {albumName}");

        HttpRequestMessage requestMessage = new(
            method: HttpMethod.Get,
            requestUri: $"search?country=US&media=music&entity=album&limit=5&term={encodedSearch}"
        );

        var responseMessage = await httpClient.SendAsync(requestMessage);

        responseMessage.EnsureSuccessStatusCode();

        using var contentStream = await responseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync(
            utf8Json: contentStream,
            jsonTypeInfo: ItunesJsonContext.Default.ApiSearchResultAlbumItem
        );
    }
}