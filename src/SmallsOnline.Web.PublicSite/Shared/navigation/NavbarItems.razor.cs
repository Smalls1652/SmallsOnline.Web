using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

using Microsoft.Extensions.Logging;

using Microsoft.JSInterop;

using SmallsOnline.Web.PublicSite.Models;

namespace SmallsOnline.Web.PublicSite.Shared.Navigation;

/// <summary>
/// Items (links) to display in the navigation bar.
/// </summary>
public partial class NavbarItems : ComponentBase, IDisposable
{
    [Inject]
    protected NavigationManager NavManager { get; set; } = null!;

    [Inject]
    protected ILogger<NavbarItems> Logger { get; set; } = null!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = null!;

    [CascadingParameter(Name = "ToggleChildCollapse")]
    public Action? ToggleChildCollapse { get; set; }

    private IJSObjectReference? navbarItemsJsModule;

    private CurrentPageLocation? _currentLocationInfo;
    private string? activeItem;
    private bool _topMusicDropDownOpened = false;

    protected override async Task OnInitializedAsync()
    {
        // Import the JS module for the component.
        navbarItemsJsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/navigation/NavbarItems.razor.js");

        // Get the current URI of the page.
        string? currentPage = GetCurrentPageFromUri(NavManager.Uri);
        Logger.LogInformation("Starting page: {currentPage}", currentPage);

        // Add the event for handling location changes.
        NavManager.LocationChanged += OnLocationChangeAsync;

        // Set the current active page.
        await SetActiveNavItemAsync(currentPage);
    }

    private async void OnLocationChangeAsync(object? sender, LocationChangedEventArgs eventArgs)
    {
        // Get the current URI of the page.
        string? currentPage = GetCurrentPageFromUri(eventArgs.Location);
        Logger.LogInformation("Page changed: {currentPage}", currentPage);
        Logger.LogInformation("Current path: {Path}", _currentLocationInfo!.Path);

        await SetActiveNavItemAsync(currentPage);
    }

    /// <summary>
    /// Parse what the current page is from a URI.
    /// </summary>
    /// <param name="uri">The input URI.</param>
    /// <returns>The current page.</returns>
    private string? GetCurrentPageFromUri(string uri)
    {
        string? currentPage = null;

        try
        {
            // Set the '_currentLocationInfo' property based off the URI.
            _currentLocationInfo = new(uri);

            // Set the current page property to the current top level page.
            currentPage = _currentLocationInfo.TopLevelPage;

            // If the current page is null, we need to set it as home. 
            if (string.IsNullOrEmpty(currentPage) == true)
            {
                currentPage = "home";
            }
        }
        catch (Exception e)
        {
            // If there's an error during parsing the URI, then log the error to the console.
            // Needs (?) to be expanded to gracefully handle errors.
            Logger.LogError("{Message}", e.Message);
        }

        // Return the current page as a string.
        // Potentially remove this?
        return currentPage;
    }

    /// <summary>
    /// Set the current active item in the navigation bar.
    /// </summary>
    /// <param name="currentPage">The current page that is active.</param>
    private async Task SetActiveNavItemAsync(string? currentPage)
    {
        if (navbarItemsJsModule != null)
        {
            // Remove the 'active' class from the previous active item.
            // Only run if 'activeItem' is not null.
            if (string.IsNullOrEmpty(activeItem) == false)
            {
                await navbarItemsJsModule.InvokeVoidAsync("removeActiveClass", $"navitem_{activeItem}");
            }

            // Set 'activeItem' to the current page.
            // Then add the 'active' class to item for the current page.
            activeItem = currentPage;
            await navbarItemsJsModule.InvokeVoidAsync("setActiveClass", $"navitem_{activeItem}");

            // Initiate a state change.
            StateHasChanged();
        }
    }

    /// <summary>
    /// Trigger the navigation bar to collapse.
    /// </summary>
    private void ToggleNavbarCollapsed()
    {
        // Set the top music dropdown menu to be inactive.
        _topMusicDropDownOpened = false;

        // Invoke the collapse up to the parent component.
        ToggleChildCollapse?.Invoke();
    }

    /// <summary>
    /// Toggle the top music dropdown menu.
    /// </summary>
    private void ToggleTopMusicDropdown()
    {
        // Flip the dropdown menu's value.
        _topMusicDropDownOpened = !_topMusicDropDownOpened;
    }

    public void Dispose()
    {
        NavManager.LocationChanged -= OnLocationChangeAsync;
    }
}