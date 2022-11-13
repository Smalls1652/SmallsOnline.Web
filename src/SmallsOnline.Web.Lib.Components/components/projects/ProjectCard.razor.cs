using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.Lib.Components.Projects;

public partial class ProjectCard : ComponentBase
{

    [Parameter, EditorRequired]
    public string Name { get; set; } = null!;

    [Parameter, EditorRequired]
    public string Type { get; set; } = null!;

    [Parameter]
    public string? Description { get; set; }

    [Parameter, EditorRequired]
    public Uri Url { get; set; } = null!;

    [Parameter]
    public bool UrlIsRepo { get; set; }

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

    private void EvaluateProjectType()
    {

        ProjectType? foundProjectType = ProjectTypes.Find(
            (ProjectType item) => item.Type == Type
        );

        if (foundProjectType is null)
        {
            throw new(
                message: $"Could not match '{Type}' from the known project types."
            );
        }

        _projectType = foundProjectType;
    }
}