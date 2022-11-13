namespace SmallsOnline.Web.Lib.Models.Blog;

public class BlogEntries : IBlogEntries
{
    public BlogEntries()
    {}

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("blogEntries")]
    public List<BlogEntry>? Entries { get; set; }
}