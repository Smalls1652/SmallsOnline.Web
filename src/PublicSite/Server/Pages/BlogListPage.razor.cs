using System.Net.Http.Json;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.PublicSite.Server;

/// <summary>
/// Page for listing blog posts.
/// </summary>
public partial class BlogListPage : ComponentBase, IDisposable
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected PersistentComponentState AppState { get; set; } = null!;

    [Inject]
    protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    /// <summary>
    /// The page number of the blog posts to display.
    /// </summary>
    [Parameter]
    public int PageNumber { get; set; } = 1;

    private bool _isFinishedLoading = false;
    private PersistingComponentStateSubscription? _persistenceSubscription;
    private BlogEntries? _blogEntries;

    private int _previousPageNumber = 1;
    private bool _previousPageBtnDisabled = false;
    private int _nextPageNumber = 1;
    private bool _nextPageBtnDisabled = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnParametersSetAsync()
    {
        // Register a persistence subscription to save the data when the page is pre-rendered.
        _persistenceSubscription = AppState.RegisterOnPersisting(PersistBlogListData);

        // If the current path is not set properly, redirect to '/blog/list/1'.
        Uri pageUri = new(NavigationManager.Uri);
        if (pageUri.AbsolutePath == "/blog" || pageUri.AbsolutePath == "/blog/" ||
            pageUri.AbsolutePath == "/blog/list" || pageUri.AbsolutePath == "/blog/list/")
        {
            NavigationManager.NavigateTo(
                uri: "/blog/list/1",
                forceLoad: false
            );
        }

        // Get the blog entries from the server.
        await GetBlogEntries();

        // Handle the previous page button.
        if (PageNumber == 1)
        {
            // If page number is 1, disable the previous page button.
            _previousPageNumber = 1;
            _previousPageBtnDisabled = true;
        }
        else
        {
            // Otherwise, set the previous page number to the current page number - 1
            // and enable the previous page button.
            _previousPageNumber = PageNumber - 1;
            _previousPageBtnDisabled = false;
        }

        // Handle the next page button.
        _nextPageNumber = PageNumber + 1;
        if (PageNumber >= _blogEntries?.TotalPages)
        {
            // If the current page number is greater than or equal to the total number of pages,
            // disable the next page button.
            _nextPageBtnDisabled = true;
        }
        else
        {
            // Otherwise, enable the next page button.
            _nextPageBtnDisabled = false;
        }

        // Set that the page has finished loading.
        _isFinishedLoading = true;

        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// Get the blog entries from the server for the current page number.
    /// </summary>
    private async Task GetBlogEntries()
    {
        // Check to see if the blog entries have already been loaded during pre-rendering.
        bool isDataAvailable = AppState.TryTakeFromJson(
            key: "blogListData",
            instance: out BlogEntries? restoredData
        );

        // Handle if the data is available.
        if (!isDataAvailable)
        {
            // If the data is not available, get the data from the server.
            using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");

            _blogEntries = await httpClient.GetFromJsonAsync(
                requestUri: $"api/blog/entries/{PageNumber}",
                jsonTypeInfo: _jsonSourceGenerationContext.BlogEntries
            );
        }
        else
        {
            // Otherwise, use the data that was persisted from pre-rendering.
            PageLogger.LogInformation(
                "Blog list data was persisted from a prerendered state. Restoring that data instead.");
            _blogEntries = restoredData;
        }
    }

    /// <summary>
    /// Persist the blog list data during pre-rendering.
    /// </summary>
    private Task PersistBlogListData()
    {
        // Persist the blog list data.
        AppState.PersistAsJson(
            key: "blogListData",
            instance: _blogEntries
        );

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_persistenceSubscription.HasValue)
            {
                _persistenceSubscription.Value.Dispose();
            }

            _blogEntries = null;
        }
    }
}