namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Holds data for a track/song returned from the iTunes Search API.
/// </summary>
public class SongItem : ISongItem
{
	/// <inheritdoc />
	[JsonPropertyName("wrapperType")]
    public string WrapperType { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("kind")]
    public string Kind { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("artistId")]
    public long ArtistId { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("collectionId")]
    public long CollectionId { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("trackId")]
    public long TrackId { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("artistName")]
    public string ArtistName { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("collectionName")]
    public string CollectionName { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("trackName")]
    public string TrackName { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("collectionCensoredName")]
    public string CollectionCensoredName { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("trackCensoredName")]
    public string TrackCensoredName { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("artistViewUrl")]
    public string ArtistViewUrl { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("collectionViewUrl")]
    public string CollectionViewUrl { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("trackViewUrl")]
    public string TrackViewUrl { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("previewUrl")]
    public string PreviewUrl { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("artworkUrl30")]
    public string ArtworkUrl30 { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("artworkUrl60")]
    public string ArtworkUrl60 { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("artworkUrl100")]
    public string ArtworkUrl100 { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("collectionPrice")]
    public double CollectionPrice { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("trackPrice")]
    public double TrackPrice { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("releaseDate")]
    public DateTimeOffset ReleaseDate { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("collectionExplicitness")]
    public string CollectionExplicitness { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("trackExplicitness")]
    public string TrackExplicitness { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("discCount")]
    public int DiscCount { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("discNumber")]
    public int DiscNumber { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("trackCount")]
    public int TrackCount { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("trackNumber")]
    public int TrackNumber { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("trackTimeMillis")]
    public long TrackTimeMillis { get; set; }

    /// <inheritdoc />
	[JsonPropertyName("country")]
    public string Country { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("currency")]
    public string Currency { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("primaryGenreName")]
    public string PrimaryGenreName { get; set; } = null!;

    /// <inheritdoc />
	[JsonPropertyName("isStreamable")]
    public bool IsStreamable { get; set; }
}