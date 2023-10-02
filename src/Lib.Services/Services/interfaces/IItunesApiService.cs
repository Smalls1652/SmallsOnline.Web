using SmallsOnline.Web.Lib.Models.Itunes;

namespace SmallsOnline.Web.Lib.Services;

public interface IItunesApiService
{
    Task<ApiSearchResult<ArtistItem>?> GetArtistSearchResultAsync(string artistName);
    Task<ApiSearchResult<ArtistItem>?> GetArtistIdLookupResultAsync(string artistId);
    Task<ApiSearchResult<SongItem>?> GetSongsByArtistResultAsync(string artistName, string songName);
    Task<ApiSearchResult<AlbumItem>?> GetAlbumsByArtistResultAsync(string artistName, string albumName);
    Task<ApiSearchResult<SongItem>?> GetSongIdLookupResultAsync(string trackId);
    Task<ApiSearchResult<AlbumItem>?> GetAlbumIdLookupResultAsync(string albumId);
}