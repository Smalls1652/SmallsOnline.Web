using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.ActivityPub;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Gets the WebFinger response from the database.
    /// </summary>
    /// <returns>The <see cref="WebFingerResponse" /> object.</returns>
    public async Task<WebFingerResponse> GetWebFingerResponseAsync()
    {
        // Get the container where the WebFinger entries are stored.
        Container container = _cosmosDbClient.GetContainer(_containerName, "webfinger");

        // There's only one entry in the database, so we can just get it by
        // using the '00000000-0000-0000-0000-000000000000' ID.
        WebFingerResponse retrievedItem = await container.ReadItemAsync<WebFingerResponse>(
            id: "00000000-0000-0000-0000-000000000000",
            partitionKey: new("mastodon-accounts")
        );

        return retrievedItem;
    }
}