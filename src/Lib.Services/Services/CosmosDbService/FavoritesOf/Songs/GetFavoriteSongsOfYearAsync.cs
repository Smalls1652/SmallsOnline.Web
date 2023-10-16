using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.CosmosDB;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <inheritdoc />
    public async Task<SongData[]> GetFavoriteSongsOfYearAsync(string listYear)
    {
        // Get the container where the favorite music entries are stored.
        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        // Define the query for getting the favorite tracks for a year.
        string coreQuery = $"WHERE c.partitionKey = \"favorites-of-tracks\" AND c.listYear = \"{listYear}\" ORDER BY c.trackReleaseDate ASC";
        QueryDefinition resultsQuery = new($"SELECT * FROM c {coreQuery}");

        // Get the count of the results that will be returned from the query.
        int resultsCount = await GetResultCount(
            container: container,
            coreQuery: coreQuery
        );

        // Instantiate an array of songs based on the count of results that will return.
        SongData[] trackItems = new SongData[resultsCount];

        // Execute the query.
        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: resultsQuery,
            requestOptions: new()
            {
                PartitionKey = new("favorites-of-tracks")
            }
        );

        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();

            CosmosDbResponse<SongData>? favoriteSongsResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: response.Content,
                jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseSongData
            );

            int i = 0;
            foreach (SongData item in favoriteSongsResponse!.Documents!)
            {
                // Add the song to the list.
                trackItems[i] = item;
                i++;
            }
        }

        return trackItems;
    }
}