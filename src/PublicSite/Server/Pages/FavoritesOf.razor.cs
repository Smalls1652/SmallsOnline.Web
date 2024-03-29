using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// The page for displaying the favorite music of a given year.
/// </summary>
public partial class FavoritesOf : ComponentBase
{
    /// <summary>
    /// Service for interacting with the Cosmos DB API.
    /// </summary>
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<FavoritesOf> PageLogger { get; set; } = null!;

    /// <summary>
    /// The list year to display data for.
    /// </summary>
    [Parameter]
    public string? ListYear { get; set; }

    private bool _isLoading = true;

    private AlbumData[]? _albumItems;
    private SongData[]? _trackItems;

    private AlbumData? _bestAlbum;
    private AlbumData[]? _favoriteAlbums;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

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
        PageLogger.LogInformation("Loading favorites of {ListYear}", ListYear);

        // Get the favorite albums from the API.
        _albumItems = await CosmosDbService.GetFavoriteAlbumsOfYearAsync(ListYear!);

        FilterFavoriteAlbumsOfYear(_albumItems);

        // Get the favorite tracks from the API.
        _trackItems = await CosmosDbService.GetFavoriteSongsOfYearAsync(ListYear!);
    }

    /// <summary>
    /// Filter favorite albums to separate the best album and the rest of the albums.
    /// </summary>
    /// <param name="inputAlbums">The collection of albums to filter.</param>
    private void FilterFavoriteAlbumsOfYear(AlbumData[]? inputAlbums)
    {
        if (inputAlbums is not null)
        {
            _bestAlbum = Array.Find(
                array: inputAlbums,
                match: (AlbumData item) => item.IsBest
            );

            _favoriteAlbums = Array.FindAll(
                array: inputAlbums,
                match: (AlbumData item) => !item.IsBest
            );
        }
    }
}