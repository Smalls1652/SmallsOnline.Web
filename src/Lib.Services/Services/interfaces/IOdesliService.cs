using SmallsOnline.Web.Lib.Models.Odesli;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Interface for the Odesli service.
/// </summary>
public interface IOdesliService
{
    /// <summary>
    /// Get the share links for a URL.
    /// </summary>
    /// <param name="inputUrl">The URL for a song/album on a music streaming service.</param>
    /// <returns>Data for the song/album from the Odesli service.</returns>
    Task<MusicEntityItem?> GetShareLinksAsync(string inputUrl);
}