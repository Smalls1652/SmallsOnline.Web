namespace SmallsOnline.Web.PublicSite.Server.Shared.Cards;

/// <summary>
/// Component for rendering a card for displaying information.
/// </summary>
public partial class InfoCard : ComponentBase
{
    [Parameter]
    public string Title { get; set; } = "⚠️ Note";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}