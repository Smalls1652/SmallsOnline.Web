namespace SmallsOnline.Web.PublicSite.Server.Shared.Sidebar;

/// <summary>
/// Component for displaying the sidebar.
/// </summary>
public partial class SidebarItemStatic : ComponentBase
{
    private readonly SocialMediaItem[] _socialMediaItems = [
        new("mastodon", "Mastodon", "https://ocw.social/@smalls", true),
        new("github", "GitHub", "https://github.com/Smalls1652", false),
        new("linkedin", "LinkedIn", "https://www.linkedin.com/in/timothy-small-a56965169", false)
    ];
}