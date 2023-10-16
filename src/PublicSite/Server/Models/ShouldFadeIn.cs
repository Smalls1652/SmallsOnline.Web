namespace SmallsOnline.Web.PublicSite.Server.Models;

/// <summary>
/// Holds the state for the "fade in" animation on page loads.
/// </summary>
[Obsolete("This class is obsolete due to changes in how Blazor renders the site now.")]
public class ShouldFadeIn
{
    /// <summary>
    /// Whether or not the "fade in" animation is enabled or not.
    /// </summary>
    public bool Enabled { get; set; } = false;
}