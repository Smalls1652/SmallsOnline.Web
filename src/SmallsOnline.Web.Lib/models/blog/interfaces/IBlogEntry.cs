namespace SmallsOnline.Web.Lib.Models.Blog;

/// <summary>
/// Interface for a specific blog entry.
/// </summary>
public interface IBlogEntry
{
    /// <summary>
    /// The unique identifier for the blog entry.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key for the blog entry in the database.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// The title of the blog entry.
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// The datetime the blog entry was created.
    /// </summary>
    DateTimeOffset? PostedDate { get; set; }

    /// <summary>
    /// The Markdown content of the blog entry.
    /// </summary>
    string? Content { get; set; }

    /// <summary>
    /// A list of tags for the blog entry.
    /// </summary>
    IEnumerable<string>? Tags { get; set; }

    /// <summary>
    /// Whether the blog entry is published or not.
    /// </summary>
    bool IsPublished { get; set; }

    /// <summary>
    /// The HTML content of the blog entry, generated from the Markdown content in the <see cref="Content"/> property.
    /// </summary>
    string? ContentHtml { get; }
}