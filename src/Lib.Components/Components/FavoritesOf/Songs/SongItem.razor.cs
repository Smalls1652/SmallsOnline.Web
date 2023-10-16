using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Components.FavoritesOf.Songs;

/// <summary>
/// Component for rendering data for a song.
/// </summary>
public partial class SongItem : ComponentBase
{
    /// <summary>
    /// The song data.
    /// </summary>
    [Parameter, EditorRequired]
    public ISongData ItemData { get; set; } = null!;

    /// <summary>
    /// Whether or not the item is being displayed on the admin site.
    /// </summary>
    [Parameter]
    public bool IsAdminSite { get; set; }
}