using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService
{
    public async Task AddOrUpdateFavoriteAlbumItemAsync(IAlbumData albumData)
    {
        // Get the container for the blog entries.
        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        using MemoryStream streamPayload = new();
        await JsonSerializer.SerializeAsync(
            utf8Json: streamPayload,
            value: albumData,
            jsonTypeInfo: CoreJsonContext.Default.AlbumData
        );

        try
        {
            // Attempt to update the item.
            await container.UpsertItemStreamAsync(
                streamPayload: streamPayload,
                partitionKey: new("favorites-of-albums")
            );
        }
        catch
        {
            await container.ReplaceItemStreamAsync(
                streamPayload: streamPayload,
                id: albumData.Id,
                partitionKey: new("favorites-of-albums")
            );
        }
    }
}