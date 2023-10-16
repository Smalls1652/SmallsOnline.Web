namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Holds data for an album returned from the iTunes Search API.
/// </summary>
public class AlbumItem : IAlbumItem
{
    /// <inheritdoc />
    [JsonPropertyName("wrapperType")]
    public string WrapperType { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("collectionType")]
    public string CollectionType { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("artistId")]
    public long ArtistId { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("collectionId")]
    public long CollectionId { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("artistName")]
    public string ArtistName { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("collectionName")]
    public string CollectionName { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("collectionCensoredName")]
    public string? CollectionCensoredName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("collectionViewUrl")]
    public string CollectionViewUrl { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("releaseDate")]
    public DateTimeOffset ReleaseDate { get; set; }
}