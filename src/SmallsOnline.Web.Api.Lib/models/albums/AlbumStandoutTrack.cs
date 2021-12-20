namespace SmallsOnline.Web.Api.Lib.Models.Albums;

public class AlbumStandoutTrack : IAlbumStandoutTrack
{
    public AlbumStandoutTrack() {}

    [JsonPropertyName("trackName")]
    public string? Name { get; set; }

    [JsonPropertyName("trackUrl")]
    public string? TrackUrl { get; set; }
}