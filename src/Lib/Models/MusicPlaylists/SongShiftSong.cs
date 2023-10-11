namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public class SongShiftSong : ISongShiftSong
{
    [JsonPropertyName("isrc")]
    public string Isrc { get; set; } = null!;

    [JsonPropertyName("serviceId")]
    public string ServiceId { get; set; } = null!;

    [JsonPropertyName("service")]
    public string Service { get; set; } = null!;

    [JsonPropertyName("artist")]
    public string Artist { get; set; } = null!;

    [JsonPropertyName("track")]
    public string Track { get; set; } = null!;

    [JsonPropertyName("album")]
    public string Album { get; set; } = null!;
}