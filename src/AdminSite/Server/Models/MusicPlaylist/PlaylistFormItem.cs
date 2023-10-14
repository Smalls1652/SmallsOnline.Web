using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Web;
using SmallsOnline.Web.Lib.Models.Database;
using SmallsOnline.Web.Lib.Models.MusicPlaylists;

namespace SmallsOnline.Web.AdminSite.Server.Models.MusicPlaylists;

public class PlaylistFormItem : DatabaseItem, IPlaylist
{
    [JsonConstructor]
    public PlaylistFormItem()
    {
    }

    public PlaylistFormItem(SongShiftPlaylistExport songShiftPlaylist)
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "music-playlist";
        Name = songShiftPlaylist.Name;
        Description = songShiftPlaylist.Description;

        if (songShiftPlaylist.Tracks is not null && songShiftPlaylist.Tracks.Length > 0)
        {
            Songs = new PlaylistSong[songShiftPlaylist.Tracks.Length];

            for (var i = 0; i < songShiftPlaylist.Tracks.Length; i++)
            {
                Songs[i] = new PlaylistSong()
                {
                    Position = i + 1,
                    ArtistName = songShiftPlaylist.Tracks[i].Artist,
                    SongName = songShiftPlaylist.Tracks[i].Track,
                    AlbumName = songShiftPlaylist.Tracks[i].Album,
                    ServiceSongId = songShiftPlaylist.Tracks[i].ServiceId
                };
            }
        }
    }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("artworkUrl")]
    public string? ArtworkUrl { get; set; }

    [JsonPropertyName("appleMusicUrl")]
    public string? AppleMusicUrl { get; set; }

    [JsonPropertyName("spotifyUrl")]
    public string? SpotifyUrl { get; set; }

    [JsonPropertyName("songs")]
    public PlaylistSong[]? Songs { get; set; }
}