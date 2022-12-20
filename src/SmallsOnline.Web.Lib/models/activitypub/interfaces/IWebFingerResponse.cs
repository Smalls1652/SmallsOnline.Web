namespace SmallsOnline.Web.Lib.Models.ActivityPub;

public interface IWebFingerResponse
{
    string Subject { get; set; }
    string[] Aliases { get; set; }
    WebFingerLink[] WebFingerLinks { get; set; }
}