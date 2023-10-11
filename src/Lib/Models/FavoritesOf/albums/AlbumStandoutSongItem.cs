namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

/// <summary>
/// Contains the data for a standout song on an album.
/// </summary>
public class AlbumStandoutSongItem : IAlbumStandoutSongItem
{
    public AlbumStandoutSongItem()
    {}

    /// <inheritdoc />
    [JsonPropertyName("songName")]
    public string? Name { get; set; }

    [JsonPropertyName("discNumber")]
    public int DiscNumber { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("songNumber")]
    public int SongNumber { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("songIsStandout")]
    public bool IsStandout { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("songUrl")]
    public string? SongUrl { get; set; }

    /// <inheritdoc />
    public string GetSongNumberAsString()
    {
        return SongNumber != 0 ? $"{SongNumber}." : string.Empty;
    }
}