namespace SmallsOnline.Web.Lib.Models.AboutMe;

public class SkillValue
{
    public SkillValue()
    {}

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}