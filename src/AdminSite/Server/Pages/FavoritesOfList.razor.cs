using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Pages;

public partial class FavoritesOfList : ComponentBase
{
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected ILogger<FavoritesOfList> PageLogger { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [CascadingParameter]
    protected Task<AuthenticationState>? AuthState { get; set; }

    [Parameter]
    public string? ListYear { get; set; }

    [Parameter]
    public string? ItemType { get; set; }

    private bool _isLoading = true;
    private bool _isLoadingData = true;

    private AlbumData[]? _albumItems;
    private SongData[]? _songItems;
    private string[]? _listYears;

    protected override async Task OnInitializedAsync()
    {
        _listYears = await CosmosDbService.GetFavoriteAlbumsListYearsAsync();
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        PageLogger.LogInformation("Retrieved list years: {ListYears}", string.Join(",", _listYears!));

        if (ListYear is not null)
        {
            await GetFavorites();

            PageLogger.LogInformation("Finished loading favorites of {ListYear}", ListYear);
        }

        _isLoading = false;

        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// Get the favorite music items from the API.
    /// </summary>
    private async Task GetFavorites()
    {
        _isLoadingData = true;
        PageLogger.LogInformation("Loading favorites of {ListYear}", ListYear);

        // Get the favorite albums from the API.
        switch (ItemType)
        {
            case "songs":
                _songItems = await CosmosDbService.GetFavoriteSongsOfYearAsync(ListYear!);
                break;

            default:
                _albumItems = await CosmosDbService.GetFavoriteAlbumsOfYearAsync(ListYear!);
                break;
        }

        _isLoadingData = false;
    }

    private void OnListYearChanged(ChangeEventArgs e)
    {
        string? listYear = e.Value?.ToString();

        if (listYear is not null)
        {
            ListYear = listYear;
            NavigationManager.NavigateTo(
                uri: $"/favorite-music-of/list/{listYear}",
                replace: true
            );

            //await GetFavorites();
        }
    }

    private void OnListItemTypeChanged(ChangeEventArgs e)
    {
        string? itemType = e.Value?.ToString();

        if (itemType is not null)
        {
            ItemType = itemType;
            NavigationManager.NavigateTo(
                uri: $"/favorite-music-of/list/{ListYear}/{itemType}",
                replace: true
            );
        }
    }
}