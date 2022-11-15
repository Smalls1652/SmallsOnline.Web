namespace SmallsOnline.Web.Lib.Models.Blog;

public interface IBlogEntries
{
    int PageNumber { get; set; }
    int TotalPages { get; set; }
    IEnumerable<BlogEntry>? Entries { get; set; }
}