namespace SmallsOnline.Web.PublicSite.Server.Models;

/// <summary>
/// Holds data for a social media profile item.
/// </summary>
public class SocialMediaItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SocialMediaItem"/> class.
    /// </summary>
    /// <param name="id">A unique identifier.</param>
    /// <param name="displayName">The display name for the item.</param>
    /// <param name="url">The URL for the item.</param>
    /// <param name="addRelMe">Whether to add the 'rel=me' attribute.</param>
    public SocialMediaItem(string id, string displayName, string url, bool addRelMe)
    {
        Id = id;
        DisplayName = displayName;
        Url = url;
        AddRelMe = addRelMe;
    }

    /// <summary>
    /// A unique identifier for the item.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The display name for the social media item.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// The URL for the social media item.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Whether to add the 'rel=me' attribute.
    /// </summary>
    public bool AddRelMe { get; set; } = false;
}