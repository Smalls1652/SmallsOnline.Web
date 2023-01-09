namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

/// <summary>
/// Interface for a favorite song.
/// </summary>
public interface ISongData
{
    string Id { get; set; }
    string PartitionKey { get; set; }

    /// <summary>
    /// The name of the song.
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// The name of the artist for the song.
    /// </summary>
    string? Artist { get; set; }

    /// <summary>
    /// The URL to the song's album art.
    /// </summary>
    string? TrackArtUrl { get; set; }

    /// <summary>
    /// The URL to song's page.
    /// </summary>
    string? TrackUrl { get; set; }

    /// <summary>
    /// The datetime the song was released.
    /// </summary>
    DateTimeOffset? ReleaseDate { get; set; }

    /// <summary>
    /// Comments about the song.
    /// </summary>
    /// <remarks>Not currently in use.</remarks>
    string? Comments { get; set; }

    /// <summary>
    /// The list year the song is in.
    /// </summary>
    string? ListYear { get; set; }

    /// <summary>
    /// Unique identifier for the song based on the artist and title.
    /// </summary>
    string? SongId { get; }
}