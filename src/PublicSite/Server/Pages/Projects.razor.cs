using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using SmallsOnline.Web.Lib.Models.Projects;
using SmallsOnline.Web.PublicSite.Server.JsonSourceGen;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// The projects page.
/// </summary>
[StreamRendering(true)]
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
            jsonTypeInfo: CoreJsonContext.Default.ProjectItemArray
        );

        _projectTypes = await JsonSerializer.DeserializeAsync(
            utf8Json: projectTypeStream,
            jsonTypeInfo: CoreJsonContext.Default.ListProjectType
        );

        _isFinishedLoading = true;
    }
}