using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Services.CosmosDB.Helpers;

namespace SmallsOnline.Web.Services.CosmosDB;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Get the favorite tracks for a specific year.
    /// </summary>
    /// <param name="listYear">The list year to get the data for.</param>
    /// <returns>A collection of favorite tracks for a year</returns>
    public async Task<IEnumerable<SongData>> GetFavoriteSongsOfYearAsync(string listYear)
    {
        // Get the container where the favorite music entries are stored.
        Container container = cosmosDbClient.GetContainer(_containerName, "favorites-of");

        // Define the query for getting the favorite tracks for a year.
        string coreQuery = $"WHERE c.partitionKey = \"favorites-of-tracks\" AND c.listYear = \"{listYear}\" ORDER BY c.trackReleaseDate ASC";
        QueryDefinition resultsQuery = new($"SELECT * FROM c {coreQuery}");

        int resultsCount = await GetResultCount(
            container: container,
            coreQuery: coreQuery
        );

        SongData[] trackItems = new SongData[resultsCount];

        // Execute the query.
        FeedIterator<SongData> containerQueryIterator = container.GetItemQueryIterator<SongData>(resultsQuery);
        while (containerQueryIterator.HasMoreResults)
        {
            int i = 0;
            foreach (SongData item in await containerQueryIterator.ReadNextAsync())
            {
                // Add the track to the list.
                trackItems[i] = item;
                i++;
            }
        }

        // Dispose of the query iterator.
        containerQueryIterator.Dispose();

        return trackItems;
    }
}