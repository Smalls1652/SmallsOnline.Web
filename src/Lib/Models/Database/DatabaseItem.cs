
namespace SmallsOnline.Web.Lib.Models.Database;

public abstract class DatabaseItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;
}