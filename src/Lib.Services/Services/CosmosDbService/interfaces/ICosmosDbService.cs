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

    Task<AlbumData[]> GetFavoriteAlbumsOfYearAsync(string listYear);
    Task<SongData[]> GetFavoriteSongsOfYearAsync(string listYear);

    Task<WebFingerResponse> GetWebFingerResponseAsync();
}