using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.Lib.Components.FavoritesOf.Albums;

public partial class AlbumItemMini : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public AlbumData ItemData { get; set; } = null!;

    [Parameter]
    public bool IsAdminSite { get; set; }
}