namespace SmallsOnline.Web.Lib.Models.Projects;

/// <summary>
/// Houses data for a project type.
/// </summary>
/// <param name="BaseType">The base type of the project.</param>
/// <param name="Type">The type of the project.</param>
/// <param name="Icon">The icon to use for the project.</param>
/// <param name="IconIsBrand">Whether the icon is a brand or not.</param>
/// <param name="IconColor">The color the icon should be.</param>
public record ProjectType(
    [property: JsonPropertyName("projectBaseType")]
    string BaseType,
    [property: JsonPropertyName("projectType")]
    string Type,
    [property: JsonPropertyName("projectIcon")]
    string Icon,
    [property: JsonPropertyName("projectIconIsBrand")]
    bool IconIsBrand,
    [property: JsonPropertyName("projectIconColor")]
    string IconColor
);