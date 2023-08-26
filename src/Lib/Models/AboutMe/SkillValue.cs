namespace SmallsOnline.Web.Lib.Models.AboutMe;

/// <summary>
/// Holds information about a single skill.
/// </summary>
public class SkillValue
{
    public SkillValue()
    {}

    /// <summary>
    /// The value of the skill.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}