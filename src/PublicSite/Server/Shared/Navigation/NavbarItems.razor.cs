using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using SmallsOnline.Web.PublicSite.Server.Models;

namespace SmallsOnline.Web.PublicSite.Server.Shared.Navigation;

/// <summary>
/// Items (links) to display in the navigation bar.
/// </summary>
public partial class NavbarItems : ComponentBase
{
    /// <summary>
    /// Dependency injected <see cref="NavigationManager"/>.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    private CurrentPageLocation? _currentPageLocation;

    protected override void OnInitialized()
    {
        _currentPageLocation = new(NavigationManager.Uri);
    }
}