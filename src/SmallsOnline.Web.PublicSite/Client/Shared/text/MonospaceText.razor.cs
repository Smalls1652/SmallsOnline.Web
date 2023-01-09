namespace SmallsOnline.Web.PublicSite.Client.Shared.Text;

/// <summary>
/// Renders text with a monospace font.
/// </summary>
public partial class MonospaceText : ComponentBase
{
    /// <summary>
    /// Content to render.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}