using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.Lib.Components.FavoritesOf.Albums;

/// <summary>
/// Component for rendering a mini version of an album.
/// </summary>
public partial class AlbumItemMini : ComponentBase
{
    /// <summary>
    /// The album data to display.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public AlbumData ItemData { get; set; } = null!;

    /// <summary>
    /// Whether or not the item is being displayed on the admin site.
    /// </summary>
    [Parameter]
    public bool IsAdminSite { get; set; }
}