using Microsoft.Azure.Cosmos;

namespace SmallsOnline.Web.Services.CosmosDB;

public partial class CosmosDbService : ICosmosDbService
{
    public async Task<WebFingerResponse> GetWebFingerResponseAsync()
    {
        // Get the container where the blog entries are stored.
        Container container = cosmosDbClient.GetContainer(_containerName, "webfinger");

        WebFingerResponse retrievedItem = await container.ReadItemAsync<WebFingerResponse>(
            id: "00000000-0000-0000-0000-000000000000",
            partitionKey: new("mastodon-accounts")
        );

        return retrievedItem;
    }
}