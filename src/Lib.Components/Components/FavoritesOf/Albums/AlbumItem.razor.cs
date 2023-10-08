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

    private int[]? _discNumbers;

    protected override void OnParametersSet()
    {
        if (ItemData.SchemaVersion == "2.0" && ItemData.OnlyStandoutSongs is not null)
        {
            _discNumbers = ItemData.OnlyStandoutSongs
                .Select(s => s.DiscNumber)
                .Distinct()
                .ToArray();
        }
    }
}