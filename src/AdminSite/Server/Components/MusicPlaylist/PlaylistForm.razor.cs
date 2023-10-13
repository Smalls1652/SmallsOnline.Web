using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.Web.AdminSite.Server.JsonSourceGen;
using SmallsOnline.Web.AdminSite.Server.Models.MusicPlaylists;
using SmallsOnline.Web.Lib.Models.Itunes;
using SmallsOnline.Web.Lib.Models.MusicPlaylists;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.AdminSite.Server.Components.MusicPlaylist;

public partial class PlaylistForm : ComponentBase
{
    [Inject]
    protected ILogger<PlaylistForm> Logger { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public PlaylistFormItem Playlist { get; set; } = null!;

    [Parameter]
    public EventCallback<PlaylistFormItem> UploadCallback { get; set; }

    private SongShiftPlaylistExport? _playlistExport;
    private bool _isUpdating;
    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(Playlist);
    }

    private async Task HandlePlaylistExportUploadAsync(InputFileChangeEventArgs e)
    {
        Logger.LogInformation("Uploading playlist export file...");
        _isUpdating = true;

        using Stream stream = e.File.OpenReadStream();

        var importedData = await JsonSerializer.DeserializeAsync(
            utf8Json: stream,
            jsonTypeInfo: CoreJsonContext.Default.SongShiftPlaylistExportArray
        );

        _playlistExport = importedData![0];

        Playlist = new(_playlistExport);

        await UploadCallback.InvokeAsync(Playlist);

        _isUpdating = false;
    }
}