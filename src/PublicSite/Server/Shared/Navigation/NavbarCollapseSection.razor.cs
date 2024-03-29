namespace SmallsOnline.Web.PublicSite.Server.Shared.Navigation;

/// <summary>
/// A collapse section in the navigation bar.
/// </summary>
public partial class NavbarCollapseSection : ComponentBase
{
    /// <summary>
    /// Whether or not the section is currently collapsed.
    /// </summary>
    [CascadingParameter(Name = "Collapsed")]
    protected bool Collapsed { get; set; }

    /// <summary>
    /// Child content to display in the collapse section.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Action for indicating the collapse section has been toggled.
    /// </summary>
    [Parameter]
    public Action? ToggleChildCollapse { get; set; }
}