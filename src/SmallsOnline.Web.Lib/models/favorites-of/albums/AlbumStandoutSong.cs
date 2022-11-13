namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

public class AlbumStandoutSong : IAlbumStandoutSong
{
    public AlbumStandoutSong() { }

    [JsonPropertyName("trackName")]
    public string? Name { get; set; }

    [JsonPropertyName("trackUrl")]
    public string? TrackUrl { get; set; }
}