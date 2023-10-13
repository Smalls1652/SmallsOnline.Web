namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public class PlaylistSong : IPlaylistSong
{
    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("artistName")]
    public string? ArtistName { get; set; }

    [JsonPropertyName("songName")]
    public string? SongName { get; set; }

    [JsonPropertyName("albumName")]
    public string? AlbumName { get; set; }

    [JsonPropertyName("albumArtUrl")]
    public string? AlbumArtUrl { get; set; }

    [JsonPropertyName("songShareUrl")]
    public string? SongShareUrl { get; set; }

    [JsonPropertyName("serviceSongId")]
    public string? ServiceSongId { get; set; }
}