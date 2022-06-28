using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.PublicSite;

public partial class BlogListPage : ComponentBase
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    public int PageNumber { get; set; } = 1;

    private bool _isFinishedLoading = false;
    private BlogEntries? _blogEntries;

    protected override async Task OnParametersSetAsync()
    {
        await GetBlogEntries();
        _isFinishedLoading = true;

        await base.OnParametersSetAsync();
    }

    private async Task GetBlogEntries()
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

        _blogEntries = await httpClient.GetFromJsonAsync<BlogEntries>($"api/blog/entries/{PageNumber}");
    }
}