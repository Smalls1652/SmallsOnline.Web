namespace SmallsOnline.Web.Services.CosmosDB;

public interface ICosmosDbService
{
    Task AddOrUpdateBlogEntryAsync(BlogEntry blogEntry);
    Task<IEnumerable<BlogEntry>> GetBlogEntriesAsync(int pageNumber = 1);
    Task<BlogEntry> GetBlogEntryAsync(string id);
    Task<int> GetBlogTotalPagesAsync();

    Task<IEnumerable<AlbumData>> GetFavoriteAlbumsOfYearAsync(string listYear);
    Task<IEnumerable<SongData>> GetFavoriteSongsOfYearAsync(string listYear);

    Task<WebFingerResponse> GetWebFingerResponseAsync();
}