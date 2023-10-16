namespace SmallsOnline.Web.Lib.Components.Core;

/// <summary>
/// Component that renders a loading bar.
/// </summary>
public partial class LoadingBar : ComponentBase
{
    /// <summary>
    /// Status text to display on the loading bar.
    /// </summary>
    [Parameter]
    public string? StatusText { get; set; }
}