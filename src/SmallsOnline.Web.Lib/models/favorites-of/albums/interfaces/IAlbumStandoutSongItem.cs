namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

/// <summary>
/// Interface for holding data for a standout song for an album.
/// </summary>
public interface IAlbumStandoutSongItem
{
    /// <summary>
    /// The name of the song.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// The position of the song on the album.
    /// </summary>
    int? SongNumber { get; set; }

    /// <summary>
    /// Whether the song is a standout song or not.
    /// </summary>
    bool IsStandout { get; set; }

    /// <summary>
    /// The URL to the song's page.
    /// </summary>
    string? SongUrl { get; set; }

    /// <summary>
    /// Get the <see cref="SongNumber" /> property as a string.
    /// </summary>
    /// <returns>The <see cref="SongNumber" /> property represented as a string.</returns>
    public string GetSongNumberAsString();
}