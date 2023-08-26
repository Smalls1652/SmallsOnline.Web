using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using SmallsOnline.Web.Lib.Models.Blog;
using Microsoft.AspNetCore.Components.Routing;
using System.Text.RegularExpressions;


namespace SmallsOnline.Web.PublicSite.Server;

/// <summary>
/// Page for rendering a specific blog post.
/// </summary>
public partial class BlogEntryPage : ComponentBase, IDisposable
{
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected PersistentComponentState AppState { get; set; } = null!;

    [Inject]
    protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// The ID of the blog post.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = null!;

    [CascadingParameter(Name = "ShouldFadeSlideIn")]
    protected ShouldFadeIn? ShouldFadeSlideIn { get; set; }

    private static readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();

    private bool _isFinishedLoading = false;
    private PersistingComponentStateSubscription? _persistenceSubscription;
    private BlogEntry? _blogEntry;
    private string? _blogExcerpt;

    private readonly Regex _anchorTagRegex = new("^(?>https|http):\\/\\/.+?\\/.*(?'anchorTag'#(?'anchorTagName'.+))$");

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnParametersSetAsync()
    {
        // Register a persistence subscription to save the data when the page is pre-rendered.
        _persistenceSubscription = AppState.RegisterOnPersisting(PersistBlogEntryData);

        // Get the blog entry data.
        await GetBlogEntry();

        // If the blog entry and the content is not null,
        // get an excerpt of the blog entry to use for the meta description.
        if (_blogEntry is not null && _blogEntry.Content is not null)
        {
            _blogExcerpt = _blogEntry.GetExcerpt(
                asPlainText: true
            );
        }

        // Set that the page has finished loading.
        _isFinishedLoading = true;

        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Run highlight.js on code blocks for syntax highlighting.
        await JSRuntime.InvokeVoidAsync("hljs.highlightAll");

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Handle the logic when the back button is clicked.
    /// </summary>
    private void HandleGoBackButtonClick()
    {
        NavigationManager.NavigateTo(
            uri: "/blog/list/1",
            forceLoad: false
        );
    }

    /// <summary>
    /// Get the blog entry data from the API.
    /// </summary>
    private async Task GetBlogEntry()
    {
        // Check to see if the blog entry has already been loaded during pre-rendering.
        bool isDataAvailable = AppState.TryTakeFromJson(
            key: "blogEntryData",
            instance: out BlogEntry? restoredData
        );

        // Handle whether the data was available or not.
        if (!isDataAvailable)
        {
            // If the data was not available, get it from the API.
            using HttpClient httpClient = HttpClientFactory.CreateClient("PublicApi");
            _blogEntry = await httpClient.GetFromJsonAsync(
                requestUri: $"api/blog/entry/{Id}",
                jsonTypeInfo: _jsonSourceGenerationContext.BlogEntry
            );
        }
        else
        {
            // Otherwise, use the data that was restored from pre-rendering.
            PageLogger.LogInformation(
                "Blog entry data was persisted from a prerendered state. Restoring that data instead.");
            _blogEntry = restoredData;
        }
    }

    /// <summary>
    /// Persist the blog entry data during pre-rendering.
    /// </summary>
    private Task PersistBlogEntryData()
    {
        AppState.PersistAsJson(
            key: "blogEntryData",
            instance: _blogEntry
        );

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_persistenceSubscription.HasValue)
            {
                // Dispose the persistence subscription, if it exists.
                _persistenceSubscription.Value.Dispose();
            }
        }
    }
}