namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

public class AlbumReleaseDateComparer : IComparer<AlbumData>
{
    public int Compare(AlbumData? album1, AlbumData? album2)
    {
        if (album1 != null && album2 != null && album1.ReleaseDate.HasValue == true && album2.ReleaseDate.HasValue == true)
        {
            return DateTimeOffset.Compare(album1.ReleaseDate.Value, album2.ReleaseDate.Value);
        }
        else
        {
            return default;
        }
    }
}