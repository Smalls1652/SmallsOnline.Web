namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

/// <summary>
/// Interface for holding the data for a standout song from an album.
/// </summary>
public interface IAlbumStandoutSong
{
    /// <summary>
    /// The name of the song.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// The URL to the song's page.
    /// </summary>
    string? TrackUrl { get; set; }
}