using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Projects;

namespace SmallsOnline.Web.PublicSite.Client;

/// <summary>
/// The projects page.
/// </summary>
public partial class Projects : ComponentBase
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }

    private static readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();

    private ProjectItem[]? _projectItems;
    private List<ProjectType>? _projectTypes;
    private bool _isFinishedLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isFinishedLoading = false;

        // Get the projects data and the types of projects.
        using (HttpClient httpClient = HttpClientFactory.CreateClient("BaseAppClient"))
        {
            _projectItems = await httpClient.GetFromJsonAsync(
                requestUri: "json/projects/projects-data.json",
                jsonTypeInfo: _jsonSourceGenerationContext.ProjectItemArray
            );
            _projectTypes = await httpClient.GetFromJsonAsync(
                requestUri: "json/projects/project-types.json",
                jsonTypeInfo: _jsonSourceGenerationContext.ListProjectType
            );
        }

        _isFinishedLoading = true;
    }
}