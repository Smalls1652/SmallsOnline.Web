using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Models.CosmosDB;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Get a blog entry by a specific ID.
    /// </summary>
    /// <param name="id">The unique ID of the blog entry.</param>
    /// <returns>A blog entry.</returns>
    public async Task<BlogEntry> GetBlogEntryAsync(string id)
    {
        // Get the container where the blog entries are stored.
        Container container = _cosmosDbClient.GetContainer(_containerName, "blogs");

        BlogEntry? retrievedItem = null;

        // Create the query to use to get the blog entry based off the URL ID.
        QueryDefinition query = new($"SELECT * FROM c WHERE c.partitionKey = \"blog-entry\" AND c.blogUrlId =\"{id}\"");

        // Execute the Cosmos DB SQL query and get the results.
        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: query,
            requestOptions: new()
            {
                PartitionKey = new("blog-entry")
            }
        );
        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();

            CosmosDbResponse<BlogEntry>? blogEntriesResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: response.Content,
                jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseBlogEntry
            );

            foreach (BlogEntry item in blogEntriesResponse!.Documents!)
            {
                retrievedItem = item;
            }
        }

        // If the blog entry wasn't found,
        // then attempt to get the blog entry with it's unique ID.
        if (retrievedItem is null)
        {
            using ResponseMessage response = await container.ReadItemStreamAsync(
                id: id,
                partitionKey: new("blog-entry")
            );

            retrievedItem = await JsonSerializer.DeserializeAsync(
                utf8Json: response.Content,
                jsonTypeInfo: CoreJsonContext.Default.BlogEntry
            );

            if (retrievedItem is null)
            {
                throw new NullReferenceException($"Blog entry with ID {id} was not found.");
            }
        }

        // If the content is not null,
        // then remove the '<!--more-->' tag from the content.
        if (retrievedItem!.Content is not null)
        {
            retrievedItem.Content = retrievedItem.Content.Replace("<!--more-->", "");
        }

        return retrievedItem;
    }
}