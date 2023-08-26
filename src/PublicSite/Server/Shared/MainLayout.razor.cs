using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace SmallsOnline.Web.PublicSite.Server.Shared;

/// <summary>
/// The main layout for the site.
/// </summary>
public partial class MainLayout : LayoutComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected ILogger<MainLayout> Logger { get; set; } = null!;

    protected override void OnInitialized()
    {
        Logger.LogInformation("Website version: {Version}", Assembly.GetExecutingAssembly().GetName().Version!.ToString());
    }

}