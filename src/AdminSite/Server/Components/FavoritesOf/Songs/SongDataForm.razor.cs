using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.Odesli;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Shared.FavoritesOf.Songs;

public partial class SongDataForm : ComponentBase
{
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected IOdesliService OdesliService { get; set; } = null!;

    [Inject]
    protected IItunesApiService ItunesApiService { get; set; } = null!;

    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected IBlobStorageService BlobStorageService { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public SongDataFormItem SongData { get; set; } = null!;

    [Parameter]
    public bool IsNewItem { get; set; }

    private EditContext? _editContext;

    private bool _isUpdating = false;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(SongData);
        base.OnInitialized();
    }

    private async Task HandleOnSubmitAsync()
    {
        _isUpdating = true;
        await CosmosDbService.AddOrUpdateFavoriteSongItemAsync(SongData.ToSongData());
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{SongData.ListYear}",
            forceLoad: false
        );
    }

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

        _isUpdating = false;
    }

    private async Task HandleArtworkUploadAsync()
    {
        _isUpdating = true;

        Regex fileExtRegex = new("^(?:https|http):\\/\\/.+\\/.+?\\.(?'fileExtension'.+?)$");
        Match fileExtMatch = fileExtRegex.Match(SongData.TrackArtUrl!);
        string fileExt = fileExtMatch.Groups["fileExtension"].Value;

        using HttpClient httpClient = HttpClientFactory.CreateClient("GenericClient");
        using HttpResponseMessage response = await httpClient.GetAsync(SongData.TrackArtUrl!);

        if (!response.IsSuccessStatusCode)
        {
            _isUpdating = false;
            return;
        }

        using Stream stream = await response.Content.ReadAsStreamAsync();

        string fileName = $"{SongData.SongId}.{fileExt}";

        SongData.TrackArtUrl = await BlobStorageService.UploadImageAsync(fileName, stream);

        _isUpdating = false;
    }

    private async Task HandleRemoveSongAsync()
    {
        _isUpdating = true;
        await CosmosDbService.RemoveFavoriteSongItemAsync(SongData.Id);
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{SongData.ListYear}",
            forceLoad: false
        );
    }
}