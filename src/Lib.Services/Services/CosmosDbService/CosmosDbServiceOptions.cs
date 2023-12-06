namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Represents the options for the CosmosDB service.
/// </summary>
public class CosmosDbServiceOptions
{
    /// <summary>
    /// Gets or sets the connection string for the CosmosDB account.
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the container in the CosmosDB.
    /// </summary>
    public string ContainerName { get; set; } = null!;
}