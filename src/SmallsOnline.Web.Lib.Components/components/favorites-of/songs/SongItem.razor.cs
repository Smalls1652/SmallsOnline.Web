using Microsoft.AspNetCore.Components;

using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.Lib.Components.FavoritesOf.Songs;

public partial class SongItem : ComponentBase
{
    [Parameter, EditorRequired]
    public ISongData ItemData { get; set; } = null!;
}