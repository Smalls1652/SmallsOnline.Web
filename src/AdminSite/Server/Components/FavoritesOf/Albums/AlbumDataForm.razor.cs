using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.Odesli;
using SmallsOnline.Web.Lib.Services;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using System.Text;

namespace SmallsOnline.Web.AdminSite.Server.Components.FavoritesOf.Albums;

/// <summary>
/// Component for rendering a form for album data.
/// </summary>
public partial class AlbumDataForm : ComponentBase
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
    /// The album data used for the form.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public AlbumDataFormItem AlbumData { get; set; } = null!;

    /// <summary>
    /// Whether the item is a new item.
    /// </summary>
    [Parameter]
    public bool IsNewItem { get; set; }

    private EditContext? _editContext;

    private bool _isUpdating = false;

    private string? _updatingStatusText;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(AlbumData);
        base.OnInitialized();
    }

    /// <summary>
    /// Handles the form submission.
    /// </summary>
    /// <returns></returns>
    private async Task HandleOnSubmitAsync()
    {
        _isUpdating = true;
        await CosmosDbService.AddOrUpdateFavoriteAlbumItemAsync(AlbumData.ToAlbumData());
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{AlbumData.ListYear}/albums",
            forceLoad: false
        );
    }

    /// <summary>
    /// Handles getting the album data from the Odesli API and the iTunes Search API.
    /// </summary>
    /// <returns></returns>
    private async Task HandleLoadAlbumDataAsync()
    {
        _isUpdating = true;

        _updatingStatusText = "Getting share links from Odesli";
        StateHasChanged();
        MusicEntityItem? odesliResult = await OdesliService.GetShareLinksAsync(AlbumData.AlbumUrl!);

        if (odesliResult is null)
        {
            _updatingStatusText = null;
            _isUpdating = false;
            return;
        }

        PlatformEntityLink itunesPlatformEntity = odesliResult.LinksByPlatform!["itunes"];
        StreamingEntityItem itunesStreamingEntity = odesliResult.EntitiesByUniqueId![itunesPlatformEntity.EntityUniqueId!];

        _updatingStatusText = "Getting album data from iTunes";
        StateHasChanged();
        ApiSearchResult<AlbumItem>? itunesAlbumData = await ItunesApiService.GetAlbumIdLookupResultAsync(itunesStreamingEntity.Id!);

        if (itunesAlbumData is null)
        {
            _updatingStatusText = null;
            _isUpdating = false;
            return;
        }

        AlbumItem? albumItem = itunesAlbumData.Results!.FirstOrDefault();

        AlbumData.Artist = albumItem?.ArtistName;
        AlbumData.Title = albumItem?.CollectionName;
        AlbumData.ReleaseDate = albumItem?.ReleaseDate;
        AlbumData.AlbumArtUrl = itunesStreamingEntity.ThumbnailUrl!.ToString();
        AlbumData.AlbumUrl = odesliResult.PageUrl!.ToString();

        await HandleArtworkUploadAsync(false);

        AlbumData.StandoutSongs = await GetAlbumSongsAsync(itunesStreamingEntity.Id!);

        _updatingStatusText = null;
        _isUpdating = false;
    }

    private async Task<AlbumStandoutSongItem[]?> GetAlbumSongsAsync(string albumId)
    {
        AlbumStandoutSongItem[]? songItems = null;
        ApiSearchResult<SongItem>? itunesSongs = await ItunesApiService.GetAlbumIdLookupSongsResultAsync(albumId);

        if (itunesSongs is not null && itunesSongs.Results is not null)
        {
            SongItem[]? songs = Array.FindAll(itunesSongs.Results, song => song.WrapperType == "track");

            songItems = new AlbumStandoutSongItem[songs.Length];

            for (int i = 0; i < songs.Length; i++)
            {
                _updatingStatusText = $"Getting data for song - {i + 1} / {songs.Length}";
                StateHasChanged();
                SongItem song = songs[i];

                MusicEntityItem? songOdesliResult = await OdesliService.GetShareLinksAsync(song.TrackViewUrl!);

                songItems[i] = new AlbumStandoutSongItem
                {
                    Name = song.TrackName,
                    DiscNumber = song.DiscNumber,
                    SongNumber = song.TrackNumber,
                    SongUrl = songOdesliResult!.PageUrl!.ToString(),
                    IsStandout = false
                };
            }
        }

        return songItems;
    }

    /// <summary>
    /// Handles uploading the artwork to the Azure Storage Account blob storage.
    /// </summary>
    /// <returns></returns>
    private async Task HandleArtworkUploadAsync() => await HandleArtworkUploadAsync(true);

    /// <summary>
    /// Handles uploading the artwork to the Azure Storage Account blob storage.
    /// </summary>
    /// <returns></returns>
    private async Task HandleArtworkUploadAsync(bool isRunningStandalone = true)
    {
        if (isRunningStandalone)
        {
            _isUpdating = true;
        }

        _updatingStatusText = "Uploading artwork to storage";
        StateHasChanged();

        Match fileExtMatch = FileExtensionRegex().Match(AlbumData.AlbumArtUrl!);
        string fileExt = fileExtMatch.Groups["fileExtension"].Value;
        string mimeType = GetImageMimeType(fileExt);

        using HttpClient httpClient = HttpClientFactory.CreateClient("GenericClient");
        using HttpResponseMessage response = await httpClient.GetAsync(AlbumData.AlbumArtUrl!);

        if (!response.IsSuccessStatusCode)
        {
            _isUpdating = false;
            return;
        }

        using Stream stream = await response.Content.ReadAsStreamAsync();

        string fileName = $"{AlbumData.AlbumId}.{fileExt}";

        AlbumData.AlbumArtUrl = await BlobStorageService.UploadImageAsync(fileName, mimeType, stream);

        _updatingStatusText = null;

        if (isRunningStandalone)
        {
            _isUpdating = false;
        }
    }

    /// <summary>
    /// Handles removing the album from the database.
    /// </summary>
    /// <returns></returns>
    private async Task HandleRemoveAlbumAsync()
    {
        _isUpdating = true;
        await CosmosDbService.RemoveFavoriteAlbumItemAsync(AlbumData.Id);
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{AlbumData.ListYear}/albums",
            forceLoad: false
        );
    }

    /// <summary>
    /// Creates an element ID for a standout song.
    /// </summary>
    /// <param name="songName">The name of the song.</param>
    /// <returns>An element ID for the song.</returns>
    private string CreateStandoutSongId(string songName)
    {
        StringBuilder sb = new();
        sb.Append("standoutsong-");
        sb.Append(Convert.ToBase64String(Encoding.Default.GetBytes(songName)));
        return sb.ToString();
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