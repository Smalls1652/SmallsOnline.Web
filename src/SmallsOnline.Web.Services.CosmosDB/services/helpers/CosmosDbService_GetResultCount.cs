using Microsoft.Azure.Cosmos;

namespace SmallsOnline.Web.Services.CosmosDB;

public partial class CosmosDbService : ICosmosDbService
{
    private async Task<int> GetResultCount(Container container, string coreQuery)
    {
        QueryDefinition countQuery = new($"SELECT VALUE COUNT(1) FROM c {coreQuery}");

        // Get the count of the results.
        FeedResponse<int> countQueryResponse = await container.GetItemQueryIterator<int>(
                queryDefinition: countQuery,
                requestOptions: new()
                {
                    MaxItemCount = 1
                }
            )
            .ReadNextAsync();

        int count = countQueryResponse.FirstOrDefault();

        return count;
    }
}