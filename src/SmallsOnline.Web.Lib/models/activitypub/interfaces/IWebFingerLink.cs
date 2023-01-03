namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Interface for ActivityPub WebFinger link.
/// </summary>
public interface IWebFingerLink
{
    string Rel { get; set; }
    string? LinkType { get; set; }
    string? Href { get; set; }
    string? Template { get; set; }
}