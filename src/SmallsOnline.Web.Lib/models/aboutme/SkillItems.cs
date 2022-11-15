using System.Collections.Generic;
using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

public class SkillItems : DatabaseItem, ISkillItems
{
    public SkillItems()
    {}

    [JsonPropertyName("itSkills")]
    public IEnumerable<SkillValue>? ITSkills { get; set; }

    [JsonPropertyName("programmingLanguages")]
    public IEnumerable<SkillValue>? ProgrammingLanguages { get; set; }
}