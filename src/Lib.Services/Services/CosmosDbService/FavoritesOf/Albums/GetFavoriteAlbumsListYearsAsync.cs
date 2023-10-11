using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.CosmosDB;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService
{
    public async Task<string[]?> GetFavoriteAlbumsListYearsAsync()
    {
        Container container = _cosmosDbClient.GetContainer(_containerName, "favorites-of");

        QueryDefinition query = new("SELECT DISTINCT VALUE c.listYear FROM c");

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: query
        );

        string[]? listYears = null;
        using ResponseMessage response = await feedIterator.ReadNextAsync();

        CosmosDbResponse<string>? listYearsResponse = await JsonSerializer.DeserializeAsync(
            utf8Json: response.Content,
            jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseString
        );

        if (listYearsResponse is null)
        {
            throw new Exception("The list years response is null.");
        }

        listYears = listYearsResponse.Documents;

        return listYears;
    }
}