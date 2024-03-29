using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService
{
    /// <inheritdoc />
    public async Task RemoveFavoriteSongItemAsync(string id)
    {
        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        await container.DeleteItemStreamAsync(
            id: id,
            partitionKey: new("favorites-of-tracks")
        );
    }
}