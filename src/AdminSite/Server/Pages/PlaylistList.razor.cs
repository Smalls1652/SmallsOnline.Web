using SmallsOnline.Web.AdminSite.Server.Models.MusicPlaylists;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.MusicPlaylists;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Pages;

public partial class PlaylistList : ComponentBase
{
    [Inject]
    protected ILogger<PlaylistList> Logger { get; set; } = null!;

    [Inject]
    protected ICosmosDbService CosmosDbService { get; set; } = null!;

    [Inject]
    protected IOdesliService OdesliService { get; set; } = null!;

    [Inject]
    protected IItunesApiService ItunesApiService { get; set; } = null!;

    private PlaylistFormItem? _playlist;

    protected override void OnInitialized()
    {
        _playlist = new();
        base.OnInitialized();
    }

    private async Task HandlePlaylistSongCallbackAsync(PlaylistFormItem playlist)
    {
        Logger.LogInformation("Received playlist song callback.");

        if (playlist is null)
        {
            Logger.LogInformation("Playlist was null.");
            return;
        }

        foreach (var song in playlist.Songs!)
        {
            Logger.LogInformation("Getting song data for {SongServiceId}.", song.ServiceSongId);

            ApiSearchResult<SongItem>? itunesSongData = await ItunesApiService.GetSongIdLookupResultAsync(song.ServiceSongId!);

            if (itunesSongData is null)
            {
                return;
            }

            song.AlbumArtUrl = itunesSongData.Results[0].ArtworkUrl100;

            var odesliSongData = await OdesliService.GetShareLinksAsync(itunesSongData.Results[0].TrackViewUrl);
            song.SongShareUrl = odesliSongData.PageUrl.ToString();
        }

        _playlist = playlist;
    }
}