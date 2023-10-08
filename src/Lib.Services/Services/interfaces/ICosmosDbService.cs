using SmallsOnline.Web.Lib.Models.ActivityPub;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public interface ICosmosDbService
{
    Task AddOrUpdateBlogEntryAsync(BlogEntry blogEntry);
    Task<BlogEntry[]> GetBlogEntriesAsync(int pageNumber = 1);
    Task<BlogEntry> GetBlogEntryAsync(string id);
    Task<int> GetBlogTotalPagesAsync();

    Task<string[]?> GetFavoriteAlbumsListYearsAsync();
    Task AddOrUpdateFavoriteAlbumItemAsync(IAlbumData albumData);
    Task<AlbumData> GetFavoriteAlbumItemAsync(string id);
    Task<AlbumData[]> GetFavoriteAlbumsOfYearAsync(string listYear);
    Task RemoveFavoriteAlbumItemAsync(string id);

    Task AddOrUpdateFavoriteSongItemAsync(ISongData songData);
    Task<SongData> GetFavoriteSongItemAsync(string id);
    Task<SongData[]> GetFavoriteSongsOfYearAsync(string listYear);
    Task RemoveFavoriteSongItemAsync(string id);

    Task<WebFingerResponse> GetWebFingerResponseAsync();
}