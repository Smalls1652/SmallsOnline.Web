namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

/// <summary>
/// Class for comparing <see cref="SongData">songs</see> by their <see cref="SongData.ReleaseDate">release date</see>.
/// </summary>
public class SongReleaseDateComparer : IComparer<SongData>
{
    /// <summary>
    /// Compares two <see cref="SongData">songs</see> by their <see cref="SongData.ReleaseDate">release date</see>.
    /// </summary>
    /// <param name="track1">The first song to compare.</param>
    /// <param name="track2">The second song to compare.</param>
    /// <returns></returns>
    public int Compare(SongData? track1, SongData? track2)
    {
        // If both songs are not null and the ReleaseDate property has a value,
        // compare them by their release date.
        if (track1 != null && track2 != null && track1.ReleaseDate.HasValue == true &&
            track2.ReleaseDate.HasValue == true)
        {
            return DateTimeOffset.Compare(track1.ReleaseDate.Value, track2.ReleaseDate.Value);
        }

        // Otherwise, do not compare them.
        return default;
    }
}