using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.CosmosDB;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Get the favorite albums for a specific year.
    /// </summary>
    /// <param name="listYear">The list year to get the data for.</param>
    /// <returns>A collection of favorite albums for a year.</returns>
    public async Task<AlbumData[]> GetFavoriteAlbumsOfYearAsync(string listYear)
    {
        // Get the container where the favorite music entries are stored.
        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        // Define the query for getting the favorite albums for a year.
        string coreQuery = $"WHERE c.partitionKey = \"favorites-of-albums\" AND c.listYear = \"{listYear}\" ORDER BY c.albumReleaseDate ASC";
        QueryDefinition resultsQuery = new($"SELECT * FROM c {coreQuery}");

        // Get the count of the results that will be returned from the query.
        int resultsCount = await GetResultCount(
            container: container,
            coreQuery: coreQuery
        );

        // Instantiate an array of albums based on the count of results that will return.
        AlbumData[] albumItems = new AlbumData[resultsCount];

        // Execute the query.
        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: resultsQuery,
            requestOptions: new()
            {
                PartitionKey = new("favorites-of-albums")
            }
        );

        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();

            CosmosDbResponse<AlbumData>? favoriteAlbumsResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: response.Content,
                jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseAlbumData
            );

            int i = 0;
            foreach (AlbumData item in favoriteAlbumsResponse!.Documents!)
            {
                // Add the album to the list.
                albumItems[i] = item;
                i++;
            }
        }

        return albumItems;
    }
}