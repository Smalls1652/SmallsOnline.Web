using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.Lib.Components.FavoritesOf.Albums;

public partial class AlbumItem : ComponentBase
{
    [Parameter, EditorRequired]
    public IAlbumData ItemData { get; set; } = null!;
}