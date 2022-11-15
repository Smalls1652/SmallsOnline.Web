namespace SmallsOnline.Web.Lib.Models.Blog;

public interface IBlogEntry
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string? Title { get; set; }
    DateTimeOffset? PostedDate { get; set; }
    string? Content { get; set; }
    IEnumerable<string>? Tags { get; set; }
    bool IsPublished { get; set; }
    string? ContentHtml { get; }
}