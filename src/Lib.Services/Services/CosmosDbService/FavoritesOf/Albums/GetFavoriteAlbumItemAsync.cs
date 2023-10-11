using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.CosmosDB;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService
{
    public async Task<AlbumData> GetFavoriteAlbumItemAsync(string id)
    {
        AlbumData? retrievedItem = null;

        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        QueryDefinition query = new($"SELECT * FROM c WHERE c.partitionKey = \"favorites-of-albums\" AND c.id = \"{id}\"");

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: query,
            requestOptions: new()
            {
                PartitionKey = new("favorites-of-albums")
            }
        );

        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();

            CosmosDbResponse<AlbumData>? databaseResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: response.Content,
                jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseAlbumData
            );

            foreach (AlbumData item in databaseResponse!.Documents!)
            {
                retrievedItem = item;
            }
        }

        if (retrievedItem is null)
        {
            throw new Exception($"Could not find the album with the ID of {id}.");
        }

        return retrievedItem;
    }
}