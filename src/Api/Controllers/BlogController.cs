using Microsoft.AspNetCore.Mvc;

namespace SmallsOnline.Web.Api.Controllers;

/// <summary>
/// API controller for blog resources.
/// </summary>
[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly ILogger<BlogController> _logger;
    private readonly ICosmosDbService _cosmosDbService;

    public BlogController(ILogger<BlogController> logger, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
    }
    
    /// <summary>
    /// Gets a paginated list of blog posts.
    /// </summary>
    /// <param name="pageNumber">The page number to get.</param>
    /// <returns>A collection of blog entries.</returns>
    [HttpGet("entries/{pageNumber?}", Name = "GetBlogEntries")]
    public async Task<BlogEntries> GetBlogEntries(int pageNumber = 1)
    {
        _logger.LogInformation("Getting blog entries.");

        // Get the total number of pages.
        int totalPages = await _cosmosDbService.GetBlogTotalPagesAsync();

        // Get the blog entries for the supplied page number.
        IEnumerable<BlogEntry> retrievedBlogEntries = await _cosmosDbService.GetBlogEntriesAsync(pageNumber);

        return new()
        {
            PageNumber = pageNumber,
            TotalPages = totalPages,
            Entries = retrievedBlogEntries as BlogEntry[]
        };
    }

    /// <summary>
    /// Get a blog entry.
    /// </summary>
    /// <param name="id">The unique ID of the blog post.</param>
    /// <returns>A blog entry.</returns>
    [HttpGet("entry/{id}", Name = "GetBlogEntry")]
    public async Task<BlogEntry> GetBlogEntry(string id)
    {
        _logger.LogInformation("Getting blog entry for id '{id}'.", id);

        // Get the blog entry for the supplied ID.
        BlogEntry retrievedBlogEntry = await _cosmosDbService.GetBlogEntryAsync(id);

        return retrievedBlogEntry;
    }
}