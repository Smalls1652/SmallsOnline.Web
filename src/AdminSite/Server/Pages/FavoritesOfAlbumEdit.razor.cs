using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Albums;
using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Pages;

public partial class FavoritesOfAlbumEdit : ComponentBase
{
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected ILogger<FavoritesOfSongEdit> PageLogger { get; set; } = null!;

    [Parameter]
    public string AlbumId { get; set; } = null!;

    [Parameter]
    public string? ListYear { get; set; }

    private bool _isLoading = true;

    private AlbumDataFormItem? _albumData;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        _albumData = (AlbumId == "new") switch
        {
            true => new(ListYear),
            _ => new(await CosmosDbService.GetFavoriteAlbumItemAsync(AlbumId))
        };

        ListYear ??= _albumData.ListYear;

        _isLoading = false;
        await base.OnParametersSetAsync();
    }
}