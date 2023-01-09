namespace SmallsOnline.Web.Lib.Models.Blog;

/// <summary>
/// Contains data for a list of <see cref="BlogEntry" /> for the current page number.
/// </summary>
public class BlogEntries : IBlogEntries
{
    public BlogEntries()
    {}

    /// <inheritdoc />
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("blogEntries")]
    public BlogEntry[]? Entries { get; set; }
}