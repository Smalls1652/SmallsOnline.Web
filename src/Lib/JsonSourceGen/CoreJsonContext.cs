using System.Text.Json.Serialization;
using SmallsOnline.Web.Lib.Models.AboutMe;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;
using SmallsOnline.Web.Lib.Models.ActivityPub;
using SmallsOnline.Web.Lib.Models.Database;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Models.Projects;
using SmallsOnline.Web.Lib.Models.MusicPlaylists;

namespace SmallsOnline.Web.Lib;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(SkillItems))]
[JsonSerializable(typeof(SkillValue))]
[JsonSerializable(typeof(WebFingerLink))]
[JsonSerializable(typeof(WebFingerResponse))]
[JsonSerializable(typeof(BlogEntries))]
[JsonSerializable(typeof(BlogEntry))]
[JsonSerializable(typeof(DatabaseItem))]
[JsonSerializable(typeof(AlbumData))]
[JsonSerializable(typeof(AlbumData[]))]
[JsonSerializable(typeof(AlbumStandoutSong))]
[JsonSerializable(typeof(AlbumStandoutSongItem))]
[JsonSerializable(typeof(SongData))]
[JsonSerializable(typeof(SongData[]))]
[JsonSerializable(typeof(ProjectItem))]
[JsonSerializable(typeof(ProjectItem[]))]
[JsonSerializable(typeof(ProjectType))]
[JsonSerializable(typeof(List<ProjectType>))]
[JsonSerializable(typeof(Playlist))]
[JsonSerializable(typeof(Playlist[]))]
[JsonSerializable(typeof(PlaylistSong))]
[JsonSerializable(typeof(PlaylistSong[]))]
[JsonSerializable(typeof(SongShiftPlaylistExport))]
[JsonSerializable(typeof(SongShiftPlaylistExport[]))]
[JsonSerializable(typeof(SongShiftSong))]
[JsonSerializable(typeof(SongShiftSong[]))]
internal partial class CoreJsonContext : JsonSerializerContext
{}