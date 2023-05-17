using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace SmallsOnline.Web.PublicSite.Client.Shared;

/// <summary>
/// The main layout for the site.
/// </summary>
public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected ILogger<MainLayout> Logger { get; set; } = null!;

    private ShouldFadeIn _shouldFadeSlideIn = new();
    private bool _isEnableFadeSlideInOnLocationChangeEventMethod;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            NavigationManager.LocationChanged += EnableFadeSlideInOnPageChange;
            _isEnableFadeSlideInOnLocationChangeEventMethod = true;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void EnableFadeSlideInOnPageChange(object? sender, LocationChangedEventArgs eventArgs)
    {
        _shouldFadeSlideIn.Enabled = true;
        StateHasChanged();
        NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
        _isEnableFadeSlideInOnLocationChangeEventMethod = false;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_isEnableFadeSlideInOnLocationChangeEventMethod)
            {
                NavigationManager.LocationChanged -= EnableFadeSlideInOnPageChange;
            }
        }
    }
}