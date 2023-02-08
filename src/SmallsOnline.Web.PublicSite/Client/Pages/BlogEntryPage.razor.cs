using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using SmallsOnline.Web.Lib.Models.Blog;
using Microsoft.AspNetCore.Components.Routing;
using System.Text.RegularExpressions;


namespace SmallsOnline.Web.PublicSite.Client;

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

    private IJSObjectReference? _blogEntryPageJSModule;
    private bool _isFinishedLoading = false;
    private PersistingComponentStateSubscription? _persistenceSubscription;
    private BlogEntry? _blogEntry;
    private string? _blogExcerpt;
    private IDisposable? _navigationChangingRegistration;

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
        if (firstRender)
        {
            // Import the JavaScript module for 'BlogEntryPage'.
            _blogEntryPageJSModule =
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/BlogEntryPage.razor.js");

            // Register the location changing handler for intercepting navigation changes to
            // footnote anchor tags on the current page.
            _navigationChangingRegistration = NavigationManager.RegisterLocationChangingHandler(InterceptAnchorTagNavigation);

            // Scroll to the anchor tag currently in the URL.
            await ScrollToAnchorAsync(NavigationManager.Uri);
        }

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

    /// <summary>
    /// Handles logic for whenever a location change is made to a footnote tag on the current page.
    /// </summary>
    /// <param name="context">Context for a change to the browser's current location.</param>
    private async ValueTask InterceptAnchorTagNavigation(LocationChangingContext context)
    {
        // If the target location starts with the current page URL,
        // then stop navigation and attempt to scroll to the footnote anchor tag.
        if (context.TargetLocation.StartsWith(NavigationManager.Uri))
        {
            // Regex pattern for getting the footnote tag in the URL.
            Regex footnoteAnchorTagRegex = new("^(?:https|http):\\/\\/.+?\\/.*(?'footnoteAnchorTag'#(?'footnoteAnchorTagName'(?:fn|fnref):.+))$");

            PageLogger.LogInformation("Target location: {Location}", context.TargetLocation);

            // Get the footnote tag.
            Match footnoteAnchorTagMatch = footnoteAnchorTagRegex.Match(context.TargetLocation);

            // If the match was a success and the value for 'footnoteAnchorTagName' is not null,
            // then scroll to the element and prevent navigation.
            if (footnoteAnchorTagMatch.Success && footnoteAnchorTagMatch.Groups["footnoteAnchorTagName"].Value is not null)
            {
                PageLogger.LogInformation("Intercepting footnote anchor tag navigation: '{FootnoteAnchorTag}'",
                    footnoteAnchorTagMatch.Groups["footnoteAnchorTagName"].Value);

                await ScrollToAnchorAsync(footnoteAnchorTagMatch.Groups["footnoteAnchorTagName"].Value);

                context.PreventNavigation();
            }
        }
    }

    /// <summary>
    /// Attempt to scroll to an anchor tag on the page.
    /// </summary>
    /// <param name="anchorTag">The anchor tag to scroll to.</param>
    private async Task ScrollToAnchorAsync(string? anchorTag)
    {
        /*
            Loop for a max of 4 times while attempting to scroll.
            We don't want to have this go on infinitely, so we're going to cap it at 4 times.
            The reason for doing this is because of a limitation of where we're handling the logic.
            If the logic was handled on the pages, we would have to define the logic on each page.
            Doing it on the main layout that handles the routing between pages, allows us to define it
            once; however, the main layout has no idea if the page has finished loading. This causes the
            initial attempt to potentially fail because the specified element doesn't exist at that point in time.
        */

        bool scrollWasSuccessful = false;
        for (int i = 1; i < 5 && scrollWasSuccessful == false; i++)
        {
            try
            {
                // Attempt to scroll to the element specified in the anchor using JavaScript interop.
                await _blogEntryPageJSModule!.InvokeVoidAsync("scrollToFootnoteAnchorId",
                    anchorTag);

                // If no exception was thrown, then set 'scrollWasSuccessful' to true to end the loop early.
                scrollWasSuccessful = true;
            }
            catch (JSException)
            {
                // If an exception was thrown while executing the JS, then attemp to log a warning message
                // and wait 1000ms (1s) to try again.
                await Task.Delay(1000);
            }
        }
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

            if (_navigationChangingRegistration is not null)
            {
                // Dispose the navigation changing registration, if it exists.
                _navigationChangingRegistration.Dispose();
            }

            _blogEntry = null;
        }
    }
}