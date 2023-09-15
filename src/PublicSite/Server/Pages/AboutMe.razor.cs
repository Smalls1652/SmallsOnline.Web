using Microsoft.AspNetCore.Components.Web;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// The about me page.
/// </summary>
[StreamRendering(true)]
public partial class AboutMe : ComponentBase
{
    protected override bool ShouldRender()
    {
        return true;
    }
}