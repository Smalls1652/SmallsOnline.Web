using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService
{
    public async Task AddOrUpdateFavoriteSongItemAsync(ISongData songData)
    {
        // Get the container for the blog entries.
        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        using MemoryStream streamPayload = new();
        await JsonSerializer.SerializeAsync(
            utf8Json: streamPayload,
            value: songData,
            jsonTypeInfo: CoreJsonContext.Default.SongData
        );

        try
        {
            // Attempt to update the item.
            await container.UpsertItemStreamAsync(
                streamPayload: streamPayload,
                partitionKey: new("favorites-of-tracks")
            );
        }
        catch
        {
            await container.ReplaceItemStreamAsync(
                streamPayload: streamPayload,
                id: songData.Id,
                partitionKey: new("favorites-of-tracks")
            );
        }
    }
}