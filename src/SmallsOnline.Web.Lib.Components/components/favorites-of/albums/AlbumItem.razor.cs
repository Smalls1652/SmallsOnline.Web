using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.Lib.Components.FavoritesOf.Albums;

/// <summary>
/// Component for rendering data for an album.
/// </summary>
public partial class AlbumItem : ComponentBase
{
    /// <summary>
    /// The album data.
    /// </summary>
    [Parameter, EditorRequired]
    public IAlbumData ItemData { get; set; } = null!;
}