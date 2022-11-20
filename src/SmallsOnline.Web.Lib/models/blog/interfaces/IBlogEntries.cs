namespace SmallsOnline.Web.Lib.Models.Blog;

public interface IBlogEntries
{
    int PageNumber { get; set; }
    int TotalPages { get; set; }
    BlogEntry[]? Entries { get; set; }
}