using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.Projects;

/// <summary>
/// Houses data for a project.
/// </summary>
public class ProjectItem : DatabaseItem, IProjectItem
{
    public ProjectItem()
    {}

    /// <inheritdoc />
    [JsonPropertyName("projectName")]
    public string? Name { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("projectDescription")]
    public string? Description { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("projectType")]
    public string? Type { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("projectUrl")]
    public Uri? Url { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("projectUrlIsRepo")]
    public bool UrlIsRepo { get; set; }
}