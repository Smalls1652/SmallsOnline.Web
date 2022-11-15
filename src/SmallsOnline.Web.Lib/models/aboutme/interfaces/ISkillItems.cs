using System.Collections.Generic;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

public interface ISkillItems
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    IEnumerable<SkillValue>? ITSkills { get; set; }
    IEnumerable<SkillValue>? ProgrammingLanguages { get; set; }
}