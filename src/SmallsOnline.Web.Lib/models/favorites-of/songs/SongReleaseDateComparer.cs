namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

public class SongReleaseDateComparer : IComparer<SongData>
{
    public int Compare(SongData? track1, SongData? track2)
    {
        if (track1 != null && track2 != null && track1.ReleaseDate.HasValue == true && track2.ReleaseDate.HasValue == true)
        {
            return DateTimeOffset.Compare(track1.ReleaseDate.Value, track2.ReleaseDate.Value);
        }
        else
        {
            return default;
        }
    }
}