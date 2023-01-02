using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.Lib.Components.Projects;

/// <summary>
/// Component for displaying data about a project.
/// </summary>
public partial class ProjectCard : ComponentBase
{
    /// <summary>
    /// The name of the project.
    /// </summary>
    [Parameter, EditorRequired]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The type of the project.
    /// </summary>
    [Parameter, EditorRequired]
    public string Type { get; set; } = null!;

    /// <summary>
    /// A description of the project.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// A URL to the project's repository or website.
    /// </summary>
    [Parameter, EditorRequired]
    public Uri Url { get; set; } = null!;

    /// <summary>
    /// Whether the project URL links to a repository or not.
    /// </summary>
    [Parameter]
    public bool UrlIsRepo { get; set; }

    // TODO: Find a way to make this ProjectType[] instead of List<ProjectType>
    /// <summary>
    /// The types of the project.
    /// </summary>
    [Parameter, EditorRequired]
    public List<ProjectType> ProjectTypes { get; set; } = null!;

    private string? _buttonText;
    private ProjectType? _projectType;

    private bool _finishedLoading = false;

    protected override void OnInitialized()
    {
        _finishedLoading = false;

        SetButtonText();
        EvaluateProjectType();

        _finishedLoading = true;
    }

    /// <summary>
    /// Set the button text based on the project type.
    /// </summary>
    private void SetButtonText()
    {
        if (UrlIsRepo == true)
        {
            _buttonText = "Visit repo";
        }
        else
        {
            _buttonText = "Visit site";
        }
    }

    /// <summary>
    /// Evaluate the project type.
    /// </summary>
    /// <exception cref="Exception">Thrown when the project's type doesn't match any known project types.</exception>
    private void EvaluateProjectType()
    {
        // Find the project type that matches the project's type.
        ProjectType? foundProjectType = ProjectTypes.Find(
            (ProjectType item) => item.Type == Type
        );

        // If the project type was not found, throw an exception.
        if (foundProjectType is null)
        {
            throw new(
                message: $"Could not match '{Type}' from the known project types."
            );
        }

        _projectType = foundProjectType;
    }
}