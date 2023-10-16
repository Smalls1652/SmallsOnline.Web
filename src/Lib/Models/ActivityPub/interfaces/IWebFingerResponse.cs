namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Interface for an ActivityPub WebFinger response.
/// </summary>
public interface IWebFingerResponse
{
    /// <summary>
    /// URI that identifies the entity in the response.
    /// </summary>
    string Subject { get; set; }

    /// <summary>
    /// URIs that identifies the entity in the response, in relation to the <see cref="Subject">Subject</see>.
    /// </summary>
    string[] Aliases { get; set; }

    /// <summary>
    /// Links that are relevant to the entity in the response.
    /// </summary>
    WebFingerLink[] WebFingerLinks { get; set; }
}