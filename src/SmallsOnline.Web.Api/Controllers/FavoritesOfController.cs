using Microsoft.AspNetCore.Mvc;

namespace SmallsOnline.Web.Api.Controllers;

/// <summary>
/// API controller for favorite music resources.
/// </summary>
[ApiController]
[Route("api/favoritesOf")]
public class FavoriteOfController : ControllerBase
{
    private readonly ILogger<FavoriteOfController> _logger;
    private readonly ICosmosDbService _cosmosDbService;

    public FavoriteOfController(ILogger<FavoriteOfController> logger, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
    }

    /// <summary>
    /// Gets all of the favorite albums for a given year.
    /// </summary>
    /// <param name="year">The year to pull data for.</param>
    /// <returns>A collection of favorite albums for the year.</returns>
    [HttpGet("albums/{year}" ,Name = "GetFavoriteAlbums")]
    public async Task<IEnumerable<AlbumData>> GetFavoriteAlbums(string year)
    {
        _logger.LogInformation("Getting favorite albums for {year}.", year);

        // Get the favorite albums for the supplied year from the database.
        List<AlbumData> retrievedAlbums = await _cosmosDbService.GetFavoriteAlbumsOfYearAsync(
            listYear: year
        );

        // Sort the albums, so that the "best" album is at the top.
        retrievedAlbums.Sort(
            (AlbumData album1, AlbumData album2) => album2.IsBest.CompareTo(album1.IsBest)
        );

        // If there retrieved albums is greater than 1 and not 0,
        // then sort the albums by release date.
        if (retrievedAlbums.Count > 1 && retrievedAlbums.Count != 0)
        {
            retrievedAlbums.Sort(
                index: 1,
                count: retrievedAlbums.Count - 1,
                comparer: new AlbumReleaseDateComparer()
            );
        }

        return retrievedAlbums.ToArray();
    }

    /// <summary>
    /// Gets all of the favorite tracks for a given year.
    /// </summary>
    /// <param name="year">The year to pull data for.</param>
    /// <returns>A collection of favorite tracks for the year.</returns>
    [HttpGet("tracks/{year}", Name = "GetFavoriteTracks")]
    public async Task<IEnumerable<SongData>> GetFavoriteTracks(string year)
    {
        _logger.LogInformation("Getting favorite tracks for {year}.", year);

        // Get the favorite tracks for the supplied year from the database.
        List<SongData> retrievedTracks = await _cosmosDbService.GetFavoriteSongsOfYearAsync(
            listYear: year
        );

        // If the retrieved tracks are not empty,
        // sort them by release date.
        if (retrievedTracks.Count > 1)
        {
            retrievedTracks.Sort(
                comparer: new SongReleaseDateComparer()
            );
        }

        return retrievedTracks.ToArray();
    }
}