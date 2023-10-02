namespace SmallsOnline.Web.Lib.Models.Itunes;

public interface IApiSearchResult<T>
{
    int ResultCount { get; set; }
    T[]? Results { get; set; }
}