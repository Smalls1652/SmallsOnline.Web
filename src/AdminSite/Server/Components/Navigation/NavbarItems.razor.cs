using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using SmallsOnline.Web.AdminSite.Server.Models;

namespace SmallsOnline.Web.AdminSite.Server.Components.Navigation;

/// <summary>
/// Items (links) to display in the navigation bar.
/// </summary>
public partial class NavbarItems : ComponentBase
{
    /// <summary>
    /// <see cref="NavigationManager"/> for the component.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    private CurrentPageLocation? _currentPageLocation;

    protected override void OnInitialized()
    {
        _currentPageLocation = new(NavigationManager.Uri);
    }
}