using System.Net;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Add or update a <see cref="BlogEntry" /> item in the database.
    /// </summary>
    /// <param name="blogEntry">The blog entry to add or update.</param>
    public async Task AddOrUpdateBlogEntryAsync(BlogEntry blogEntry)
    {
        // Get the container for the blog entries.
        Container container = _cosmosDbClient.GetContainer(_containerName, "blogs");

        try
        {
            // Attempt to update the item.
            await container.ReplaceItemAsync(
                item: blogEntry,
                id: blogEntry.Id,
                partitionKey: new("blog-entry")
            );
        }
        catch (CosmosException dbException)
        {
            if (dbException.StatusCode == HttpStatusCode.NotFound)
            {
                // If the status code is 'NotFound',
                // then create the item.
                await container.CreateItemAsync(
                item: blogEntry,
                partitionKey: new("blog-entry")
            );
            }
        }
    }
}