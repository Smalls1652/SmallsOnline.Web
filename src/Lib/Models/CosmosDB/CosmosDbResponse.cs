namespace SmallsOnline.Web.Lib.Models.CosmosDB;

public class CosmosDbResponse<T> : ICosmosDbResponse<T>
{
    [JsonPropertyName("Documents")]
    public T[]? Documents { get; set; }

    [JsonPropertyName("_count")]
    public int Count { get; set; }
}