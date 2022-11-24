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
        IEnumerable<AlbumData> retrievedAlbums = await _cosmosDbService.GetFavoriteAlbumsOfYearAsync(
            listYear: year
        );

        return retrievedAlbums;
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
        IEnumerable<SongData> retrievedTracks = await _cosmosDbService.GetFavoriteSongsOfYearAsync(
            listYear: year
        );

        return retrievedTracks;
    }
}