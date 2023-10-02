using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Albums;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.Odesli;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Shared.FavoritesOf.Albums;

public partial class AlbumDataForm : ComponentBase
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
    public AlbumDataFormItem AlbumData { get; set; } = null!;

    [Parameter]
    public bool IsNewItem { get; set; }

    private EditContext? _editContext;

    private bool _isUpdating = false;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(AlbumData);
        base.OnInitialized();
    }

    private async Task HandleOnSubmitAsync()
    {
        _isUpdating = true;
        await CosmosDbService.AddOrUpdateFavoriteAlbumItemAsync(AlbumData.ToAlbumData());
        NavigationManager.NavigateTo(
            uri: $"/favorite-music-of/list/{AlbumData.ListYear}",
            forceLoad: false
        );
    }

    private async Task HandleLoadAlbumDataAsync()
    {
        _isUpdating = true;

        MusicEntityItem? odesliResult = await OdesliService.GetShareLinksAsync(AlbumData.AlbumUrl!);

        if (odesliResult is null)
        {
            _isUpdating = false;
            return;
        }

        PlatformEntityLink itunesPlatformEntity = odesliResult.LinksByPlatform!["itunes"];
        StreamingEntityItem itunesStreamingEntity = odesliResult.EntitiesByUniqueId![itunesPlatformEntity.EntityUniqueId!];

        ApiSearchResult<AlbumItem>? itunesAlbumData = await ItunesApiService.GetAlbumIdLookupResultAsync(itunesStreamingEntity.Id!);

        if (itunesAlbumData is null)
        {
            _isUpdating = false;
            return;
        }

        AlbumItem? albumItem = itunesAlbumData.Results!.FirstOrDefault();

        AlbumData.Artist = albumItem?.ArtistName;
        AlbumData.Title = albumItem?.CollectionName;
        AlbumData.ReleaseDate = albumItem?.ReleaseDate;
        AlbumData.AlbumArtUrl = itunesStreamingEntity.ThumbnailUrl!.ToString();
        AlbumData.AlbumUrl = odesliResult.PageUrl!.ToString();

        _isUpdating = false;
    }

    private async Task HandleArtworkUploadAsync()
    {
        _isUpdating = true;

        Regex fileExtRegex = new("^(?:https|http):\\/\\/.+\\/.+?\\.(?'fileExtension'.+?)$");
        Match fileExtMatch = fileExtRegex.Match(AlbumData.AlbumArtUrl!);
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

        _isUpdating = false;
    }
}