namespace SmallsOnline.Web.Lib.Components.Core;

/// <summary>
/// Component that renders a loading bar.
/// </summary>
public partial class LoadingBar : ComponentBase
{
    [Parameter]
    public string? StatusText { get; set; }
}