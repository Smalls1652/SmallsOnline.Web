namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Interface for a song returned from the iTunes Search API.
/// </summary>
public interface ISongItem
{
    /// <summary>
    /// The type of object returned.
    /// </summary>
    /// <remarks>
    /// Values typically returned: track, collection, artist
    /// </remarks>
    string WrapperType { get; set; }

    /// <summary>
    /// The kind of content returned.
    /// </summary>
    string Kind { get; set; }

    /// <summary>
    /// The unique identifier for the artist.
    /// </summary>
    long ArtistId { get; set; }

    /// <summary>
    /// The unique identifier for the collection/album.
    /// </summary>
    long CollectionId { get; set; }

    /// <summary>
    /// The unique identifier for the track/song.
    /// </summary>
    long TrackId { get; set; }

    /// <summary>
    /// The name of the artist.
    /// </summary>
    string ArtistName { get; set; }

    /// <summary>
    /// The name of the collection/album.
    /// </summary>
    string CollectionName { get; set; }

    /// <summary>
    /// The name of the track/song.
    /// </summary>
    string TrackName { get; set; }

    /// <summary>
    /// The name of the collection/album, if it's censored.
    /// </summary>
    string CollectionCensoredName { get; set; }

    /// <summary>
    /// The name of the track/song, if it's censored.
    /// </summary>
    string TrackCensoredName { get; set; }

    /// <summary>
    /// The artist's iTunes URL.
    /// </summary>
    string ArtistViewUrl { get; set; }

    /// <summary>
    /// The collection/album's iTunes URL.
    /// </summary>
    string CollectionViewUrl { get; set; }

    /// <summary>
    /// The track/song's iTunes URL.
    /// </summary>
    string TrackViewUrl { get; set; }

    /// <summary>
    /// The preview URL for the track/song.
    /// </summary>
    string PreviewUrl { get; set; }

    /// <summary>
    /// The 30x30 artwork URL for the track/song.
    /// </summary>
    string ArtworkUrl30 { get; set; }

    /// <summary>
    /// The 60x60 artwork URL for the track/song.
    /// </summary>
    string ArtworkUrl60 { get; set; }

    /// <summary>
    /// The 100x100 artwork URL for the track/song.
    /// </summary>
    string ArtworkUrl100 { get; set; }

    /// <summary>
    /// The price for the collection/album.
    /// </summary>
    double CollectionPrice { get; set; }

    /// <summary>
    /// The price for the track/song.
    /// </summary>
    double TrackPrice { get; set; }

    /// <summary>
    /// The release date for the track/song.
    /// </summary>
    DateTimeOffset ReleaseDate { get; set; }

    /// <summary>
    /// Whether the collection/album is explicit.
    /// </summary>
    string CollectionExplicitness { get; set; }

    /// <summary>
    /// Whether the track/song is explicit.
    /// </summary>
    string TrackExplicitness { get; set; }

    /// <summary>
    /// The disc count for the collection/album.
    /// </summary>
    int DiscCount { get; set; }

    /// <summary>
    /// The disc number for the track/song on the collection/album.
    /// </summary>
    int DiscNumber { get; set; }

    /// <summary>
    /// The count of tracks/songs on the collection/album.
    /// </summary>
    int TrackCount { get; set; }

    /// <summary>
    /// The position for the track/song on the collection/album.
    /// </summary>
    int TrackNumber { get; set; }

    /// <summary>
    /// The length of the track/song in milliseconds.
    /// </summary>
    long TrackTimeMillis { get; set; }

    /// <summary>
    /// The country for the track/song.
    /// </summary>
    string Country { get; set; }

    /// <summary>
    /// The currency used for purchasing the track/song.
    /// </summary>
    string Currency { get; set; }

    /// <summary>
    /// The primary genre name for the track/song.
    /// </summary>
    string PrimaryGenreName { get; set; }

    /// <summary>
    /// Whether the track/song is streamable on Apple Music.
    /// </summary>
    bool IsStreamable { get; set; }
}