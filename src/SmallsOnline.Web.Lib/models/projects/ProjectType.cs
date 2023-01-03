namespace SmallsOnline.Web.Lib.Models.Projects;

/// <summary>
/// Houses data for a project type.
/// </summary>
/// <param name="BaseType"></param>
/// <param name="Type"></param>
/// <param name="Icon"></param>
/// <param name="IconIsBrand"></param>
/// <param name="IconColor"></param>
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