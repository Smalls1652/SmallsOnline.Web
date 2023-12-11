using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.Odesli;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Components.FavoritesOf.Songs;

/// <summary>
/// Component for rendering a form for song data.
/// </summary>
public partial class SongDataForm : ComponentBase
{
    /// <summary>
    /// Service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    /// <summary>
    /// Service for interacting with the Odesli API.
    /// </summary>
    [Inject]
    protected IOdesliService OdesliService { get; set; } = null!;

    /// <summary>
    /// Service for interacting with the iTunes Search API.
    /// </summary>
    [Inject]
    protected IItunesApiService ItunesApiService { get; set; } = null!;

    /// <summary>
    /// Dependency injected <see cref="IHttpClientFactory" /> for creating <see cref="HttpClient" /> instances.
    /// </summary>
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    /// <summary>
    /// Service for interacting with the Azure Storage Account blob storage.
    /// </summary>
    [Inject]
    protected IBlobStorageService BlobStorageService { get; set; } = null!;

    /// <summary>
    /// <see cref="NavigationManager" /> for the component.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// The song data used for the form.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public SongDataFormItem SongData { get; set; } = null!;

    /// <summary>
    /// Whether or not the item is a new item.
    /// </summary>
    [Parameter]
    public bool IsNewItem { get; set; }

    private EditContext? _editContext;

    private bool _isUpdating = false;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(SongData);
        base.OnInitialized();
    }

    /// <summary>
    /// Handle the form being submitted.
    /// </summary>
    /// <returns></returns>
    private async Task HandleOnSubmitAsync()
    {
        _isUpdating = true;
        await CosmosDbService.AddOrUpdateFavoriteSongItemAsync(SongData.ToSongData());
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{SongData.ListYear}",
            forceLoad: false
        );
    }

    /// <summary>
    /// Handle loading song data from the Odesli API and the iTunes Search API.
    /// </summary>
    /// <returns></returns>
    private async Task HandleLoadSongDataAsync()
    {
        _isUpdating = true;

        MusicEntityItem? odesliResult = await OdesliService.GetShareLinksAsync(SongData.TrackUrl!);

        if (odesliResult is null)
        {
            _isUpdating = false;
            return;
        }

        PlatformEntityLink itunesPlatformEntity = odesliResult.LinksByPlatform!["itunes"];
        StreamingEntityItem itunesStreamingEntity = odesliResult.EntitiesByUniqueId![itunesPlatformEntity.EntityUniqueId!];

        ApiSearchResult<SongItem>? itunesSongData = await ItunesApiService.GetSongIdLookupResultAsync(itunesStreamingEntity.Id!);

        if (itunesSongData is null)
        {
            _isUpdating = false;
            return;
        }

        SongItem? songItem = itunesSongData.Results!.FirstOrDefault();

        SongData.Artist = songItem?.ArtistName;
        SongData.Title = songItem?.TrackName;
        SongData.ReleaseDate = songItem?.ReleaseDate;
        SongData.TrackArtUrl = itunesStreamingEntity.ThumbnailUrl!.ToString();
        SongData.TrackUrl = odesliResult.PageUrl!.ToString();

        await HandleArtworkUploadAsync(false);

        _isUpdating = false;
    }

    /// <summary>
    /// Handles uploading the artwork to blob storage.
    /// </summary>
    /// <returns></returns>
    private async Task HandleArtworkUploadAsync() => await HandleArtworkUploadAsync(true);

    /// <summary>
    /// Handle uploading the artwork to blob storage.
    /// </summary>
    /// <returns></returns>
    private async Task HandleArtworkUploadAsync(bool isRunningStandalone = true)
    {
        if (isRunningStandalone)
        {
            _isUpdating = true;
        }

        Match fileExtMatch = FileExtensionRegex().Match(SongData.TrackArtUrl!);
        string fileExt = fileExtMatch.Groups["fileExtension"].Value;
        string mimeType = GetImageMimeType(fileExt);

        using HttpClient httpClient = HttpClientFactory.CreateClient("GenericClient");
        using HttpResponseMessage response = await httpClient.GetAsync(SongData.TrackArtUrl!);

        if (!response.IsSuccessStatusCode)
        {
            _isUpdating = false;
            return;
        }

        using Stream stream = await response.Content.ReadAsStreamAsync();

        string fileName = $"{SongData.SongId}.{fileExt}";

        SongData.TrackArtUrl = await BlobStorageService.UploadImageAsync(fileName, mimeType, stream);

        if (isRunningStandalone)
        {
            _isUpdating = false;
        }
    }

    /// <summary>
    /// Handle removing the song from the database.
    /// </summary>
    /// <returns></returns>
    private async Task HandleRemoveSongAsync()
    {
        _isUpdating = true;
        await CosmosDbService.RemoveFavoriteSongItemAsync(SongData.Id);
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{SongData.ListYear}",
            forceLoad: false
        );
    }

    [GeneratedRegex(
        pattern: "^(?:https|http):\\/\\/.+\\/.+?\\.(?'fileExtension'.+?)$"
    )]
    private static partial Regex FileExtensionRegex();

    private static string GetImageMimeType(string fileExtension)
    {
        string mimeType = fileExtension switch
        {
            "png" => "image/png",
            "jpg" or ".jpeg" => "image/jpeg",
            "gif" => "image/gif",
            "bmp" => "image/bmp",
            "tiff" => "image/tiff",
            "ico" => "image/x-icon",
            "svg" => "image/svg+xml",
            "webp" => "image/webp",
            "avif" => "image/avif",
            "heif" => "image/heif",
            "heic" => "image/heic",
            _ => throw new NotSupportedException($"The image file extension '{fileExtension}' is not supported.")
        };

        return mimeType;
    }
}