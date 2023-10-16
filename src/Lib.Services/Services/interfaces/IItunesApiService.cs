using SmallsOnline.Web.Lib.Models.Itunes;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Interface for the iTunes Search API service.
/// </summary>
public interface IItunesApiService
{
    /// <summary>
    /// Perform a search for an artist.
    /// </summary>
    /// <param name="artistName">The name of the artist.</param>
    /// <returns>A collection of artists returned by the API.</returns>
    Task<ApiSearchResult<ArtistItem>?> GetArtistSearchResultAsync(string artistName);

    /// <summary>
    /// Lookup an artist by their ID.
    /// </summary>
    /// <param name="artistId">The unique identifier for the artist.</param>
    /// <returns>The artist returned by the API.</returns>
    Task<ApiSearchResult<ArtistItem>?> GetArtistIdLookupResultAsync(string artistId);

    /// <summary>
    /// Perform a search for a song(s) by an artist.
    /// </summary>
    /// <param name="artistName">The name of the artist.</param>
    /// <param name="songName">The name of the song.</param>
    /// <returns>A collection of songs by the artist returned by the API.</returns>
    Task<ApiSearchResult<SongItem>?> GetSongsByArtistResultAsync(string artistName, string songName);

    /// <summary>
    /// Perform a search for an album(s) by an artist.
    /// </summary>
    /// <param name="artistName">The name of the artist.</param>
    /// <param name="albumName">The name of the album.</param>
    /// <returns>A collection of albums by the artist returned by the API.</returns>
    Task<ApiSearchResult<AlbumItem>?> GetAlbumsByArtistResultAsync(string artistName, string albumName);

    /// <summary>
    /// Lookup a song by its ID.
    /// </summary>
    /// <param name="trackId">The unique identifier for the song.</param>
    /// <returns>The song returned by the API.</returns>
    Task<ApiSearchResult<SongItem>?> GetSongIdLookupResultAsync(string trackId);

    /// <summary>
    /// Lookup an album by its ID.
    /// </summary>
    /// <param name="albumId">The unique identifier for the album.</param>
    /// <returns>The album returned by the API.</returns>
    Task<ApiSearchResult<AlbumItem>?> GetAlbumIdLookupResultAsync(string albumId);

    /// <summary>
    /// Lookup the songs for an album by its ID.
    /// </summary>
    /// <param name="albumId">The unique identifier for the album.</param>
    /// <returns>A collection of songs for an album returned by the API.</returns>
    Task<ApiSearchResult<SongItem>?> GetAlbumIdLookupSongsResultAsync(string albumId);
}