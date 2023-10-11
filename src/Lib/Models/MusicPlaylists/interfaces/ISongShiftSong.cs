namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public interface ISongShiftSong
{
    string Isrc { get; set; }
    string ServiceId { get; set; }
    string Service { get; set; }
    string Artist { get; set; }
    string Track { get; set; }
    string Album { get; set; }
}