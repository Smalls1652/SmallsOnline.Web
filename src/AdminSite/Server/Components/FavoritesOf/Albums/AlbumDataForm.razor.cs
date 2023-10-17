using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Albums;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.Odesli;
using SmallsOnline.Web.Lib.Services;
using System.Collections.Generic;
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
            uri: $"/favorite-music-of/list/{AlbumData.ListYear}",
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

        if (AlbumData.SchemaVersion == "2.0")
        {
            ApiSearchResult<SongItem>? itunesSongs = await ItunesApiService.GetAlbumIdLookupSongsResultAsync(itunesStreamingEntity.Id!);

            if (itunesSongs is not null && itunesSongs.Results is not null)
            {
                SongItem[]? songs = Array.FindAll(itunesSongs.Results, song => song.WrapperType == "track");

                AlbumStandoutSongItem[] songItems = new AlbumStandoutSongItem[songs.Length];

                for (int i = 0; i < songs.Length; i++)
                {
                    _updatingStatusText = $"Getting data for song: {i + 1} / {songs.Length}";
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

                AlbumData.StandoutSongs = songItems;
            }
        }

        _updatingStatusText = null;
        _isUpdating = false;
    }

    /// <summary>
    /// Handles uploading the artwork to the Azure Storage Account blob storage.
    /// </summary>
    /// <returns></returns>
    private async Task HandleArtworkUploadAsync()
    {
        _isUpdating = true;

        _updatingStatusText = "Uploading artwork to storage";

        Match fileExtMatch = FileExtensionRegex().Match(AlbumData.AlbumArtUrl!);
        string fileExt = fileExtMatch.Groups["fileExtension"].Value;

        using HttpClient httpClient = HttpClientFactory.CreateClient("GenericClient");
        using HttpResponseMessage response = await httpClient.GetAsync(AlbumData.AlbumArtUrl!);

        if (!response.IsSuccessStatusCode)
        {
            _isUpdating = false;
            return;
        }

        using Stream stream = await response.Content.ReadAsStreamAsync();

        string fileName = $"{AlbumData.AlbumId}.{fileExt}";

        AlbumData.AlbumArtUrl = await BlobStorageService.UploadImageAsync(fileName, stream);

        _updatingStatusText = null;
        _isUpdating = false;
    }

    /// <summary>
    /// Handles adding a standout song to the album.
    /// </summary>
    private void HandleAddStandoutSong()
    {
        AlbumData.AddStandoutSong();
        StateHasChanged();
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
            uri: $"/favorite-music-of/list/{AlbumData.ListYear}",
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
}