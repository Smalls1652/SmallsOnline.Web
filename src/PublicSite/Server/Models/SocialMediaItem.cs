namespace SmallsOnline.Web.PublicSite.Server.Models;

public class SocialMediaItem
{
    public SocialMediaItem(string id, string displayName, string url, bool addRelMe)
    {
        Id = id;
        DisplayName = displayName;
        Url = url;
        AddRelMe = addRelMe;
    }

    public string Id { get; set; }

    public string DisplayName { get; set; }

    public string Url { get; set; }

    public bool AddRelMe { get; set; } = false;
}