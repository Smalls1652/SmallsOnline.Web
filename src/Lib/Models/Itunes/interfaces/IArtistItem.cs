namespace SmallsOnline.Web.Lib.Models.Itunes;

public interface IArtistItem
{
    /// <summary>
    /// The type of object returned.
    /// </summary>
    /// <remarks>
    /// Values typically returned: track, collection, artist
    /// </remarks>
    string WrapperType { get; set; }

    /// <summary>
    /// The type of the artist object.
    /// </summary>
    string ArtistType { get; set; }

    /// <summary>
    /// The name of the artist.
    /// </summary>
    string ArtistName { get; set; }

    /// <summary>
    /// The artist's iTunes URL.
    /// </summary>
    string ArtistLinkUrl { get; set; }

    /// <summary>
    /// The unique identifier for the artist.
    /// </summary>
    long ArtistId { get; set; }

    /// <summary>
    /// The AMG Artist ID for the artist.
    /// </summary>
    long? AmgArtistId { get; set; }

    /// <summary>
    /// The primary genre name for the artist.
    /// </summary>
    string PrimaryGenreName { get; set; }

    /// <summary>
    /// The uniqie identifier for the artist's primary genre.
    /// </summary>
    long PrimaryGenreId { get; set; }
}