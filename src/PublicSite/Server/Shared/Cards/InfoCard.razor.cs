namespace SmallsOnline.Web.PublicSite.Server.Shared.Cards;

/// <summary>
/// Component for rendering a card for displaying information.
/// </summary>
public partial class InfoCard : ComponentBase
{
    /// <summary>
    /// The title for the info card.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "⚠️ Note";

    /// <summary>
    /// Child content to display in the info card.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}