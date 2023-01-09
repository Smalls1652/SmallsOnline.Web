namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

/// <summary>
/// Hold data for standout song for an album.
/// </summary>
public class AlbumStandoutSong : IAlbumStandoutSong
{
    public AlbumStandoutSong() { }

    /// <inheritdoc />
    [JsonPropertyName("trackName")]
    public string? Name { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackUrl")]
    public string? TrackUrl { get; set; }
}