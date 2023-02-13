using System.Collections.Generic;

namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

/// <summary>
/// Interface for holding data for a favorite album.
/// </summary>
public interface IAlbumData
{
    string Id { get; set; }
    string PartitionKey { get; set; }

    /// <summary>
    /// The schema version of the data.
    /// </summary>
    string? SchemaVersion { get; set; }

    /// <summary>
    /// The title of the album.
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// The artist of the album.
    /// </summary>
    string? Artist { get; set; }

    /// <summary>
    /// A collection of <see cref="AlbumStandoutSongItem">standout songs</see> for the album.
    /// </summary>
    AlbumStandoutSongItem[]? StandoutSongs { get; set; }

    /// <summary>
    /// A collection of only the <see cref="AlbumStandoutSongItem">standout songs</see> for the album.
    /// </summary>
    IEnumerable<AlbumStandoutSongItem>? OnlyStandoutSongs { get; }

    /// <summary>
    /// A collection of <see cref="AlbumSongItem">standout tracks</see> for the album.
    /// </summary>
    /// <remarks>Used for legacy data.</remarks>
    IEnumerable<AlbumStandoutSong>? StandoutTracks { get; set; }

    /// <summary>
    /// The URL to the album's artwork.
    /// </summary>
    string? AlbumArtUrl { get; set; }

    /// <summary>
    /// The URL to the album's share page.
    /// </summary>
    string? AlbumUrl { get; set; }

    /// <summary>
    /// Whether or not it is the best album for a specific year.
    /// </summary>
    bool IsBest { get; set; }

    /// <summary>
    /// Comments about the album.
    /// </summary>
    /// <remarks>Not currently in use.</remarks>
    string? Comments { get; set; }

    /// <summary>
    /// The datetime the album was released.
    /// </summary>
    DateTimeOffset? ReleaseDate { get; set; }

    /// <summary>
    /// The list year the album is in.
    /// </summary>
    string? ListYear { get; set; }

    /// <summary>
    /// Unique identifier for the album based on the artist and title.
    /// </summary>
    string? AlbumId { get; }
}