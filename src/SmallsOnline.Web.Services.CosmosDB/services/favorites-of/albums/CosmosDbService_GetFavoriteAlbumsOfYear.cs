using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Services.CosmosDB.Helpers;

namespace SmallsOnline.Web.Services.CosmosDB;

public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// Get the favorite albums for a specific year.
    /// </summary>
    /// <param name="listYear">The list year to get the data for.</param>
    /// <returns>A collection of favorite albums for a year.</returns>
    public async Task<IEnumerable<AlbumData>> GetFavoriteAlbumsOfYearAsync(string listYear)
    {
        // Get the container where the favorite music entries are stored.
        Container container = cosmosDbClient.GetContainer(_containerName, "favorites-of");

        // Define the query for getting the favorite albums for a year.
        string coreQuery = $"WHERE c.partitionKey = \"favorites-of-albums\" AND c.listYear = \"{listYear}\"";
        QueryDefinition resultsQuery = new($"SELECT * FROM c {coreQuery}");

        int resultsCount = await GetResultCount(
            container: container,
            coreQuery: coreQuery
        );

        AlbumData[] albumItems = new AlbumData[resultsCount];

        // Execute the query.
        FeedIterator<AlbumData> containerQueryIterator = container.GetItemQueryIterator<AlbumData>(resultsQuery);

        while (containerQueryIterator.HasMoreResults)
        {
            int i = 0;
            foreach (AlbumData item in await containerQueryIterator.ReadNextAsync())
            {
                // Add the album to the list.
                albumItems[i] = item;
                i++;
            }
        }

        // Dispose of the query iterator.
        containerQueryIterator.Dispose();

        return albumItems;
    }
}