using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.Json;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Service for interacting with Cosmos DB.
/// </summary>
public partial class CosmosDbService : ICosmosDbService
{
    /// <summary>
    /// The CosmosDB client.
    /// </summary>
    private readonly CosmosClient _cosmosDbClient;

    /// <summary>
    /// Instantiate the service with service with a connection string and the database name.
    /// </summary>
    /// <param name="connectionString">The connection string for authenticating the service.</param>
    /// <param name="containerName">The database name to connect to.</param>
    public CosmosDbService(string connectionString, string containerName)
    {
        _containerName = containerName;
        _cosmosDbClient = InitService(connectionString, jsonSerializer);
    }

    private readonly string _containerName;

    /// <summary>
    /// The JSON serializer for the CosmosDB client.
    /// </summary>
    private readonly CosmosDbSerializer jsonSerializer = new(
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new JsonDateTimeOffsetConverter()
            }
        }
    );

    /// <summary>
    /// Create a CosmosDB client.
    /// </summary>
    /// <param name="dbSerializer">The JSON serializer to use for the CosmosDB client.</param>
    /// <returns>A CosmosDB client.</returns>
    private static CosmosClient InitService(string connectionString, CosmosDbSerializer dbSerializer)
    {
        return new(
            connectionString: connectionString,
            clientOptions: new()
            {
                Serializer = dbSerializer
            }
        );
    }
}