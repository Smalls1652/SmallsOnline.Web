using System.Collections.Generic;

namespace SmallsOnline.Web.Lib.Models.AboutMe;

/// <summary>
/// Interface for containing skills.
/// </summary>
public interface ISkillItems
{
    /// <summary>
    /// Unique identifier for the database.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Partition key for the database.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// List of skills related to IT.
    /// </summary>
    SkillValue[]? ITSkills { get; set; }

    /// <summary>
    /// List of skills related to programming.
    /// </summary>
    SkillValue[]? ProgrammingLanguages { get; set; }
}