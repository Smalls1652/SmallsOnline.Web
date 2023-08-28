using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// The projects page.
/// </summary>
public partial class Projects : ComponentBase
{
    private ProjectItem[]? _projectItems;
    private List<ProjectType>? _projectTypes;
    private bool _isFinishedLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isFinishedLoading = false;

        await using var projectItemStream = File.Open(Path.Combine(Environment.CurrentDirectory, "Data/projects/projects-data.json"), FileMode.Open);
        await using var projectTypeStream = File.Open(Path.Combine(Environment.CurrentDirectory, "Data/projects/project-types.json"), FileMode.Open);

        _projectItems = await JsonSerializer.DeserializeAsync(
            utf8Json: projectItemStream,
            jsonTypeInfo: JsonSourceGenerationContext.Default.ProjectItemArray
        );

        _projectTypes = await JsonSerializer.DeserializeAsync(
            utf8Json: projectTypeStream,
            jsonTypeInfo: JsonSourceGenerationContext.Default.ListProjectType
        );

        _isFinishedLoading = true;
    }
}