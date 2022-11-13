namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

public interface IAlbumStandoutSongItem
{
    string? Name { get; set; }
    int? SongNumber { get; set; }
    bool IsStandout { get; set; }
    string? SongUrl { get; set; }

    public string GetSongNumberAsString();
}