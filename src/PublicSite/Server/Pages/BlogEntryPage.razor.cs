using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using SmallsOnline.Web.Lib.Models.Blog;
using Microsoft.AspNetCore.Components.Routing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
using SmallsOnline.Web.Lib.Services;


namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// Page for rendering a specific blog post.
/// </summary>
public partial class BlogEntryPage : ComponentBase
{
    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected ILogger<BlogEntryPage> PageLogger { get; set; } = null!;

    [Inject]
    protected IJSRuntime JsRuntime { get; set; } = null!;

    /// <summary>
    /// The ID of the blog post.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = null!;

    private bool _isFinishedLoading = false;
    private BlogEntry? _blogEntry;
    private string? _blogExcerpt;

    private readonly Regex _anchorTagRegex = new("^(?>https|http):\\/\\/.+?\\/.*(?'anchorTag'#(?'anchorTagName'.+))$");

    protected override async Task OnInitializedAsync()
    {
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
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Run highlight.js on code blocks for syntax highlighting.
        await JsRuntime.InvokeVoidAsync("hljs.highlightAll");

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Get the blog entry data from the API.
    /// </summary>
    private async Task GetBlogEntry()
    {
        _blogEntry = await CosmosDbService.GetBlogEntryAsync(Id);
    }
}