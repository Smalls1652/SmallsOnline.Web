using System.Net;
using System.Text.Json;
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

        using MemoryStream streamPayload = new();
        await JsonSerializer.SerializeAsync(
            utf8Json: streamPayload,
            value: blogEntry,
            jsonTypeInfo: CoreJsonContext.Default.BlogEntry
        );

        try
        {
            // Attempt to update the item.
            await container.UpsertItemStreamAsync(
                streamPayload: streamPayload,
                partitionKey: new("blog-entry")
            );
        }
        catch
        {
            await container.ReplaceItemStreamAsync(
                streamPayload: streamPayload,
                id: blogEntry.Id,
                partitionKey: new("blog-entry")
            );
        }
    }
}