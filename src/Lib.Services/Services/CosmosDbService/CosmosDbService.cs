using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
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

    private readonly ILogger<CosmosDbService> _logger;

    /// <summary>
    /// Create an instance of the <see cref="CosmosDbService"/>.
    /// </summary>
    /// <param name="connectionString">The connection string for authenticating the service.</param>
    /// <param name="containerName">The database name to connect to.</param>
    public CosmosDbService(string connectionString, string containerName)
    {
        _containerName = containerName;
        _cosmosDbClient = InitService(connectionString);
        _logger = NullLogger<CosmosDbService>.Instance;
    }

    /// <summary>
    /// Create an instance of the <see cref="CosmosDbService"/>.
    /// </summary>
    /// <param name="options">The <see cref="CosmosDbServiceOptions"/> for configuring the service.</param>
    public CosmosDbService(IOptions<CosmosDbServiceOptions> options)
    {
        _containerName = options.Value.ContainerName;
        _cosmosDbClient = InitService(options.Value.ConnectionString);
        _logger = NullLogger<CosmosDbService>.Instance;
    }

    public CosmosDbService(IOptions<CosmosDbServiceOptions> options, ILogger<CosmosDbService>? logger)
    {
        _containerName = options.Value.ContainerName;
        _cosmosDbClient = InitService(options.Value.ConnectionString);

        if (options.Value.EnableLogging)
        {
            _logger = logger ?? NullLogger<CosmosDbService>.Instance;
        }
        else
        {
            _logger = NullLogger<CosmosDbService>.Instance;
        }
    }

    private readonly string _containerName;

    /// <summary>
    /// Create a CosmosDB client.
    /// </summary>
    /// <param name="connectionString">The connection string for authenticating the service.</param>
    /// <returns>A CosmosDB client.</returns>
    private static CosmosClient InitService(string connectionString)
    {
        return new(
            connectionString: connectionString
        );
    }
}