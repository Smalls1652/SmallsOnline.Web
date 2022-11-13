using System.Collections.Generic;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

public interface ISkillItems
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    List<SkillValue>? ITSkills { get; set; }
    List<SkillValue>? ProgrammingLanguages { get; set; }
}