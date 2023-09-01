namespace SmallsOnline.Web.Lib.Models.CosmosDB;

public interface ICosmosDbResponse<T>
{
    T[]? Documents { get; set; }

    int Count { get; set; }
}