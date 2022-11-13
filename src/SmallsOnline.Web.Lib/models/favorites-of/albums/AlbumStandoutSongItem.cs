namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

public class AlbumStandoutSongItem : IAlbumStandoutSongItem
{
    public AlbumStandoutSongItem()
    {}

    [JsonPropertyName("songName")]
    public string? Name { get; set; }

    [JsonPropertyName("songNumber")]
    public int? SongNumber { get; set; }

    [JsonPropertyName("songIsStandout")]
    public bool IsStandout { get; set; }

    [JsonPropertyName("songUrl")]
    public string? SongUrl { get; set; }

    public string GetSongNumberAsString()
    {
        return SongNumber is not null && SongNumber != 0 ? $"{SongNumber}." : string.Empty;
    }
}