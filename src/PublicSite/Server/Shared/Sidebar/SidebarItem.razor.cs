namespace SmallsOnline.Web.PublicSite.Server.Shared.Sidebar;

/// <summary>
/// Component for displaying the sidebar.
/// </summary>
public partial class SidebarItem : ComponentBase
{
    [Inject]
    protected IConfiguration Configuration { get; set; } = null!;

    private SocialMediaItem[] _socialMediaItems = new SocialMediaItem[]
    {
        new("mastodon", "Mastodon", "https://ocw.social/@smalls", true),
        new("github", "GitHub", "https://github.com/Smalls1652", false),
        new("linkedin", "LinkedIn", "https://www.linkedin.com/in/timothy-small-a56965169", false)
    };

    private readonly string _mastodonUrl = "https://ocw.social/@smalls";

    private bool _isDevelopmentMode;

    protected override void OnInitialized()
    {
        _isDevelopmentMode = Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}