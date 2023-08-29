namespace SmallsOnline.Web.PublicSite.Server.Shared.Sidebar;

/// <summary>
/// Component for displaying the sidebar.
/// </summary>
public partial class SidebarItem : ComponentBase
{
    [Inject]
    protected IConfiguration Configuration { get; set; } = null!;

    private readonly string _mastodonUrl = "https://ocw.social/@smalls";

    private bool _isDevelopmentMode;

    protected override void OnInitialized()
    {
        _isDevelopmentMode = Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}