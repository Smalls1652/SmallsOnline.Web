namespace SmallsOnline.Web.PublicSite.Server.Models;

/// <summary>
/// Holds the state for the favorites of pages.
/// </summary>
[Obsolete("This class is obsolete due to changes in how Blazor renders the site now.")]
public class FavoritesOfStateContainer
{
    private string? _savedListYear;

    public string? ListYear
    {
        get => _savedListYear;
        set
        {
            _savedListYear = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}