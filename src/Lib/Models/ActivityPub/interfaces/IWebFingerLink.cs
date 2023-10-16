namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Interface for an ActivityPub WebFinger link.
/// </summary>
public interface IWebFingerLink
{
    /// <summary>
    /// The link relation type
    /// </summary>
    string Rel { get; set; }

    /// <summary>
    /// The media type for the link.
    /// </summary>
    string? LinkType { get; set; }

    /// <summary>
    /// The URL for the link.
    /// </summary>
    string? Href { get; set; }

    /// <summary>
    /// The template for the link.
    /// </summary>
    string? Template { get; set; }
}