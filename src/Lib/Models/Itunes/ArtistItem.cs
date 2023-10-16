namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Holds data for an artist returned from the iTunes Search API.
/// </summary>
public class ArtistItem : IArtistItem
{
    /// <inheritdoc />
    [JsonPropertyName("wrapperType")]
    public string WrapperType { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("artistType")]
    public string ArtistType { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("artistName")]
    public string ArtistName { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("artistLinkUrl")]
    public string ArtistLinkUrl { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("artistId")]
    public long ArtistId { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("amgArtistId")]
    public long? AmgArtistId { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("primaryGenreName")]
    public string PrimaryGenreName { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("primaryGenreId")]
    public long PrimaryGenreId { get; set; }
}