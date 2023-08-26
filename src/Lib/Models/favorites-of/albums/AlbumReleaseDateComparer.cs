namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

/// <summary>
/// Class for comparing <see cref="AlbumData">albums</see> by their <see cref="AlbumData.ReleaseDate">release date</see>.
/// </summary>
/// <remarks>Used for sorting albums.</remarks>
public class AlbumReleaseDateComparer : IComparer<AlbumData>
{
    /// <summary>
    /// Compares two <see cref="AlbumData">albums</see> by their <see cref="AlbumData.ReleaseDate">release date</see>.
    /// </summary>
    /// <param name="album1">The first album to compare.</param>
    /// <param name="album2">The second album to compare.</param>
    /// <returns></returns>
    public int Compare(AlbumData? album1, AlbumData? album2)
    {
        // If both albums are not null and the ReleaseDate property has a value,
        // compare them by their release date.
        if (album1 != null && album2 != null && album1.ReleaseDate.HasValue == true &&
            album2.ReleaseDate.HasValue == true)
        {
            return DateTimeOffset.Compare(album1.ReleaseDate.Value, album2.ReleaseDate.Value);
        }

        // Otherwise, do not compare them.
        return default;
    }
}