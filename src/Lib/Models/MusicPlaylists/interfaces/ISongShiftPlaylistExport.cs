namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public interface ISongShiftPlaylistExport
{
    string Service { get; set; }
    string ServiceId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    SongShiftSong[]? Tracks { get; set; }
}