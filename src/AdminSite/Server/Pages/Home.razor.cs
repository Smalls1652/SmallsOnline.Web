using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace SmallsOnline.Web.AdminSite.Server.Pages;

/// <summary>
/// The index/home page.
/// </summary>
public partial class Home : ComponentBase
{
    [CascadingParameter]
    protected Task<AuthenticationState>? AuthState { get; set; }
}