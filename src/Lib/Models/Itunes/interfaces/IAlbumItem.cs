namespace SmallsOnline.Web.Lib.Models.Itunes;

/// <summary>
/// Interface for an album returned by the iTunes Search API.
/// </summary>
public interface IAlbumItem
{
    /// <summary>
    /// The type of object returned.
    /// </summary>
    /// <remarks>
    /// Values typically returned: track, collection, artist
    /// </remarks>
    string WrapperType { get; set; }

    /// <summary>
    /// The type of collection returned.
    /// </summary>
    string CollectionType { get; set; }

    /// <summary>
    /// The unique identifier for the artist.
    /// </summary>
    long ArtistId { get; set; }

    /// <summary>
    /// The unique identifier for the collection/album.
    /// </summary>
    long CollectionId { get; set; }

    /// <summary>
    /// The name of the artist.
    /// </summary>
    string ArtistName { get; set; }

    /// <summary>
    /// The name of the collection/album.
    /// </summary>
    string CollectionName { get; set; }

    /// <summary>
    /// The name of the collection/album, if it's censored.
    /// </summary>
    string? CollectionCensoredName { get; set; }

    /// <summary>
    /// The collection/album's iTunes URL.
    /// </summary>
    string CollectionViewUrl { get; set; }

    /// <summary>
    /// The release date for the collection/album.
    /// </summary>
    DateTimeOffset ReleaseDate { get; set; }
}