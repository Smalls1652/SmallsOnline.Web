namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

public interface ISongData
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string? Title { get; set; }
    string? Artist { get; set; }
    string? TrackArtUrl { get; set; }
    string? TrackUrl { get; set; }
    DateTimeOffset? ReleaseDate { get; set; }
    string? Comments { get; set; }
    string? ListYear { get; set; }
    string? SongId { get; }
}