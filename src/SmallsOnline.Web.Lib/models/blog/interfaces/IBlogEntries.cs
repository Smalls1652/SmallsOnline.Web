namespace SmallsOnline.Web.Lib.Models.Blog;

/// <summary>
/// Interface for blog entries.
/// </summary>
public interface IBlogEntries
{
    /// <summary>
    /// The current page for number for the set.
    /// </summary>
    int PageNumber { get; set; }

    /// <summary>
    /// The total number of pages available.
    /// </summary>
    int TotalPages { get; set; }

    /// <summary>
    /// The blog entries for the current page number.
    /// </summary>
    BlogEntry[]? Entries { get; set; }
}