using System.Net.Http.Json;
using System.Runtime.InteropServices;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// Page for listing blog posts.
/// </summary>
public partial class BlogListPage : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    /// <summary>
    /// The page number of the blog posts to display.
    /// </summary>
    [Parameter]
    public int PageNumber { get; set; } = 1;

    private bool _isFinishedLoading = false;
    private BlogEntries? _blogEntries;

    private int _previousPageNumber = 1;
    private bool _previousPageBtnDisabled = false;
    private int _nextPageNumber = 1;
    private bool _nextPageBtnDisabled = false;

    protected override async Task OnParametersSetAsync()
    {
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
        int totalPages = await CosmosDbService.GetBlogTotalPagesAsync();
        BlogEntry[] retrievedBlogEntries = await CosmosDbService.GetBlogEntriesAsync(PageNumber);

        _blogEntries = new()
        {
            Entries = retrievedBlogEntries,
            PageNumber = PageNumber,
            TotalPages = totalPages
        };
    }
}