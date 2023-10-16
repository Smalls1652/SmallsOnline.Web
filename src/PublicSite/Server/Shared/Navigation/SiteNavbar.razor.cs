namespace SmallsOnline.Web.PublicSite.Server.Shared.Navigation;

/// <summary>
/// The navigation bar for displaying branding and links to pages.
/// </summary>
public partial class SiteNavbar : ComponentBase
{
    /// <summary>
    /// Logger for the <see cref="SiteNavbar"/> component.
    /// </summary>
    [Inject]
    protected ILogger<SiteNavbar> Logger { get; set; } = null!;
}