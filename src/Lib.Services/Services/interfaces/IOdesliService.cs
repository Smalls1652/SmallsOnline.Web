using SmallsOnline.Web.Lib.Models.Odesli;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Interface for the Odesli service.
/// </summary>
public interface IOdesliService
{
    Task<MusicEntityItem?> GetShareLinksAsync(string inputUrl);
}