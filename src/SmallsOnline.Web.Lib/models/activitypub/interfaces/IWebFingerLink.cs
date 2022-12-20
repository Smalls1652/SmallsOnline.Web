namespace SmallsOnline.Web.Lib.Models.ActivityPub;

public interface IWebFingerLink
{
    string Rel { get; set; }
    string? LinkType { get; set; }
    string? Href { get; set; }
    string? Template { get; set; }
}