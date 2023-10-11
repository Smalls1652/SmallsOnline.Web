using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.CosmosDB;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService
{
    public async Task<SongData> GetFavoriteSongItemAsync(string id)
    {
        SongData? retrievedItem = null;

        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        QueryDefinition query = new($"SELECT * FROM c WHERE c.partitionKey = \"favorites-of-tracks\" AND c.id = \"{id}\"");

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: query,
            requestOptions: new()
            {
                PartitionKey = new("favorites-of-tracks")
            }
        );

        using ResponseMessage response = await feedIterator.ReadNextAsync();

        CosmosDbResponse<SongData>? databaseResponse = await JsonSerializer.DeserializeAsync(
            utf8Json: response.Content,
            jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseSongData
        );

        foreach (SongData item in databaseResponse!.Documents!)
        {
            retrievedItem = item;
        }


        if (retrievedItem is null)
        {
            throw new Exception($"Could not find the song with the ID of {id}.");
        }

        return retrievedItem!;
    }
}