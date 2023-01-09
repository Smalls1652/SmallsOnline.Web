using System.Collections.Generic;
using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

/// <summary>
/// Holds skill items for various skill types.
/// </summary>
public class SkillItems : DatabaseItem, ISkillItems
{
    public SkillItems()
    {}

    /// <inheritdoc />
    [JsonPropertyName("itSkills")]
    public SkillValue[]? ITSkills { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("programmingLanguages")]
    public SkillValue[]? ProgrammingLanguages { get; set; }
}