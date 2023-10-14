namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public interface IPlaylist
{
    string? Name { get; set; }
    string? Description { get; set; }
    string? ArtworkUrl { get; set; }
    string? AppleMusicUrl { get; set; }
    string? SpotifyUrl { get; set; }
    PlaylistSong[]? Songs { get; set; }
}