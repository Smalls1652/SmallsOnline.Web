using SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Pages;

public partial class FavoritesOfSongEdit : ComponentBase
{
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected ILogger<FavoritesOfSongEdit> PageLogger { get; set; } = null!;

    [Parameter]
    public string SongId { get; set; } = null!;

    [Parameter]
    public string? ListYear { get; set; }

    private bool _isLoading = true;

    private SongDataFormItem? _songItem;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        _songItem = (SongId == "new") switch
        {
            true => new(ListYear),
            _ => new(await CosmosDbService.GetFavoriteSongItemAsync(SongId))
        };

        if (_songItem.ListYear is not null)
        {
            ListYear = _songItem.ListYear;
        }

        _isLoading = false;
        await base.OnParametersSetAsync();
    }
}