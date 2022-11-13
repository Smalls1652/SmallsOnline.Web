namespace SmallsOnline.Web.Lib.Models.Projects;

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