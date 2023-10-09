namespace SmallsOnline.Web.AdminSite.Server.Shared.Navigation;

/// <summary>
/// The navigation bar for displaying branding and links to pages.
/// </summary>
public partial class SiteNavbar : ComponentBase
{
    [Inject]
    protected ILogger<SiteNavbar> Logger { get; set; } = null!;
}