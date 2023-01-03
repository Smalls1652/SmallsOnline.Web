namespace SmallsOnline.Web.Lib.Models.Projects;

/// <summary>
/// Interface for a project item.
/// </summary>
public interface IProjectItem
{
    /// <summary>
    /// The ID of the project.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key of the project in the database.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// The name of the project.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// The description of the project.
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// The type of the project.
    /// </summary>
    string? Type { get; set; }

    /// <summary>
    /// The URL of the project's repository or website.
    /// </summary>
    Uri? Url { get; set; }

    /// <summary>
    /// Whether the project's URL is a link to a repository or not.
    /// </summary>
    bool UrlIsRepo { get; set; }
}