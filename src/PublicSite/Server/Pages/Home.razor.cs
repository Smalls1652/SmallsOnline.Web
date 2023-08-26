namespace SmallsOnline.Web.PublicSite.Server;

/// <summary>
/// The index/home page.
/// </summary>
public partial class Home : ComponentBase
{
    [Inject]
    protected ILogger<Home> PageLogger { get; set; } = null!;
}