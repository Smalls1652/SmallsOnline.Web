using System.Collections.Generic;

namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

public interface IAlbumData
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string? SchemaVersion { get; set; }
    string? Title { get; set; }
    string? Artist { get; set; }
    List<AlbumStandoutSongItem>? StandoutSongs { get; set; }
    List<AlbumStandoutSongItem>? OnlyStandoutSongs { get; }
    List<AlbumStandoutSong> StandoutTracks { get; set; }
    string? AlbumArtUrl { get; set; }
    string? AlbumUrl { get; set; }
    bool IsBest { get; set; }
    string? Comments { get; set; }
    DateTimeOffset? ReleaseDate { get; set; }
    string? ListYear { get; set; }
    string? AlbumId { get; }
}