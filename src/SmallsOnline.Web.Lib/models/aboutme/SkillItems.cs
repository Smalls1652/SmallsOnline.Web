using System.Collections.Generic;
using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

public class SkillItems : DatabaseItem, ISkillItems
{
    public SkillItems()
    {}

    [JsonPropertyName("itSkills")]
    public SkillValue[]? ITSkills { get; set; }

    [JsonPropertyName("programmingLanguages")]
    public SkillValue[]? ProgrammingLanguages { get; set; }
}