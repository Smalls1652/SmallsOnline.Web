namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Contains the data for a WebFinger response.
/// </summary>
public class WebFingerResponse : IWebFingerResponse
{
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = null!;

    [JsonPropertyName("aliases")]
    public string[] Aliases { get; set; } = null!;

    [JsonPropertyName("links")]
    public WebFingerLink[] WebFingerLinks { get; set; } = null!;
}