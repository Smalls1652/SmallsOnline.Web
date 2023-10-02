using System.Text.Json.Serialization;

namespace SmallsOnline.Web.Lib.Models.Itunes;

public class AlbumItem : IAlbumItem
{
    [JsonPropertyName("wrapperType")]
    public string WrapperType { get; set; } = null!;

    [JsonPropertyName("collectionType")]
    public string CollectionType { get; set; } = null!;

    [JsonPropertyName("artistId")]
    public long ArtistId { get; set; }

    [JsonPropertyName("collectionId")]
    public long CollectionId { get; set; }

    [JsonPropertyName("artistName")]
    public string ArtistName { get; set; } = null!;

    [JsonPropertyName("collectionName")]
    public string CollectionName { get; set; } = null!;

    [JsonPropertyName("collectionCensoredName")]
    public string? CollectionCensoredName { get; set; }

    [JsonPropertyName("collectionViewUrl")]
    public string CollectionViewUrl { get; set; } = null!;

    [JsonPropertyName("releaseDate")]
    public DateTimeOffset ReleaseDate { get; set; }
}