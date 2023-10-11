namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public interface IPlaylistSong
{
    int Position { get; set; }
    string? ArtistName { get; set; }
    string? SongName { get; set; }
    string? AlbumName { get; set; }
    string? AlbumArtUrl { get; set; }
    string? SongShareUrl { get; set; }
}