namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Interface for an iTunes Search API result.
/// </summary>
/// <typeparam name="T">The type of the search result returned.</typeparam>
public interface IApiSearchResult<T>
{
    /// <summary>
    /// The count of results returned.
    /// </summary>
    int ResultCount { get; set; }

    /// <summary>
    /// Results returned from the API.
    /// </summary>
    T[]? Results { get; set; }
}