namespace SmallsOnline.Web.PublicSite.Client.Shared.Text;

/// <summary>
/// Renders text with a monospace font.
/// </summary>
public partial class MonospaceText : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}