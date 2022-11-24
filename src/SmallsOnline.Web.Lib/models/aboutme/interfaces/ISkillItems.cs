using System.Collections.Generic;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

public interface ISkillItems
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    SkillValue[]? ITSkills { get; set; }
    SkillValue[]? ProgrammingLanguages { get; set; }
}