namespace SmallsOnline.Web.Lib.Models.MusicPlaylists;

public class SongShiftPlaylistExport : ISongShiftPlaylistExport
{
    [JsonPropertyName("service")]
    public string Service { get; set; } = null!;

    [JsonPropertyName("serviceId")]
    public string ServiceId { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("tracks")]
    public SongShiftSong[]? Tracks { get; set; }
}