using System.Text.Json.Serialization;

namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Holds data for a search result returned from the iTunes Search API.
/// </summary>
/// <typeparam name="T">The type of the search result returned.</typeparam>
public class ApiSearchResult<T> : IApiSearchResult<T>
{
    /// <inheritdoc />
    [JsonPropertyName("resultCount")]
    public int ResultCount { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("results")]
    public T[]? Results { get; set; }
}