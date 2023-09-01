using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Get the total number of pages for blog entries.
    /// </summary>
    /// <returns>The total number of pages available.</returns>
    public async Task<int> GetBlogTotalPagesAsync()
    {
        // Get the container where the blog entries are stored.
        Container container = _cosmosDbClient.GetContainer(_containerName, "blogs");

        // Define the query for getting the total number of blog entries.
        // The query creates a column for the total number of distinct blog entries.
        QueryDefinition query = new("SELECT DISTINCT VALUE n.itemCount FROM (SELECT COUNT(1) AS itemCount FROM c WHERE c.partitionKey = 'blog-entry' AND c.blogIsPublished = true ) n");

        // Run the query.
        // Note: Due to the weird way Cosmos DB handles queries, the query must be run through a loop.
        using FeedIterator<int> countQueryIterator = container.GetItemQueryIterator<int>(
            queryDefinition: query,
            requestOptions: new()
            {
                PartitionKey = new("blog-entry"),
                MaxItemCount = 1
            }
        );

        FeedResponse<int> countQueryResponse = await countQueryIterator.ReadNextAsync();
        int totalPagesCount = countQueryResponse.FirstOrDefault();

        // Return the total number of pages by,
        // dividing the total number of blog entries by the number of items per page
        // and rounding up to the nearest whole number.
        return (int)Math.Round((decimal)totalPagesCount / 5, 0, MidpointRounding.ToPositiveInfinity);
    }
}