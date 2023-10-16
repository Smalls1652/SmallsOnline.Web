using SmallsOnline.Web.Lib.Models.ActivityPub;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Interface for an Azure Cosmos DB service.
/// </summary>
public interface ICosmosDbService
{
    /// <summary>
    /// Add or update a blog entry in the database.
    /// </summary>
    /// <param name="blogEntry">The blog entry to add or update.</param>
    /// <returns></returns>
    Task AddOrUpdateBlogEntryAsync(BlogEntry blogEntry);

    /// <summary>
    /// Get a list of blog entries from the database.
    /// </summary>
    /// <param name="pageNumber">The page number to return.</param>
    /// <returns>A collection of blog entries.</returns>
    Task<BlogEntry[]> GetBlogEntriesAsync(int pageNumber = 1);

    /// <summary>
    /// Get a blog entry from the database.
    /// </summary>
    /// <param name="id">The unique identifier for the blog entry.</param>
    /// <returns>The blog entry stored in the database.</returns>
    Task<BlogEntry> GetBlogEntryAsync(string id);

    /// <summary>
    /// Get the total number of pages of blog entries in the database.
    /// </summary>
    /// <returns>The total number of pages.</returns>
    Task<int> GetBlogTotalPagesAsync();

    /// <summary>
    /// Get the unique list years for favorite albums/songs in the database.
    /// </summary>
    /// <returns>A collection of list years.</returns>
    Task<string[]?> GetFavoriteAlbumsListYearsAsync();

    /// <summary>
    /// Add or update a favorite album in the database.
    /// </summary>
    /// <param name="albumData">The album data to add or update.</param>
    /// <returns></returns>
    Task AddOrUpdateFavoriteAlbumItemAsync(IAlbumData albumData);

    /// <summary>
    /// Get a favorite album from the database.
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    /// <returns>The album data for the item.</returns>
    Task<AlbumData> GetFavoriteAlbumItemAsync(string id);

    /// <summary>
    /// Get all of the favorite albums for a given list year.
    /// </summary>
    /// <param name="listYear">The list year.</param>
    /// <returns>A collection of albums for the given list year.</returns>
    Task<AlbumData[]> GetFavoriteAlbumsOfYearAsync(string listYear);

    /// <summary>
    /// Remove a favorite album from the database.
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    /// <returns></returns>
    Task RemoveFavoriteAlbumItemAsync(string id);

    /// <summary>
    /// Add or update a favorite song in the database.
    /// </summary>
    /// <param name="songData">The song data to add or update.</param>
    /// <returns></returns>
    Task AddOrUpdateFavoriteSongItemAsync(ISongData songData);

    /// <summary>
    /// Get a favorite song from the database.
    /// </summary>
    /// <param name="id">The unique identifier for the song.</param>
    /// <returns>The song data for the item.</returns>
    Task<SongData> GetFavoriteSongItemAsync(string id);

    /// <summary>
    /// Get all of the favorite songs for a given list year.
    /// </summary>
    /// <param name="listYear">The list year.</param>
    /// <returns>A collection of songs for the given list year.</returns>
    Task<SongData[]> GetFavoriteSongsOfYearAsync(string listYear);

    /// <summary>
    /// Remove a favorite song from the database.
    /// </summary>
    /// <param name="id">The unique identifier for the song.</param>
    /// <returns></returns>
    Task RemoveFavoriteSongItemAsync(string id);

    /// <summary>
    /// Get the WebFinger response for the domain from the database.
    /// </summary>
    /// <returns>The WebFinger response.</returns>
    Task<WebFingerResponse> GetWebFingerResponseAsync();
}