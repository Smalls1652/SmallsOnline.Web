using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public class Playlist : DatabaseItem, IPlaylist
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("artworkUrl")]
    public string? ArtworkUrl { get; set; }

    [JsonPropertyName("appleMusicUrl")]
    public string? AppleMusicUrl { get; set; }

    [JsonPropertyName("spotifyUrl")]
    public string? SpotifyUrl { get; set; }

    [JsonPropertyName("songs")]
    public PlaylistSong[]? Songs { get; set; }
}