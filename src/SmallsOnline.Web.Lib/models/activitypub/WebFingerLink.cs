namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Contains data for a WebFinger link.
/// </summary>
public class WebFingerLink : IWebFingerLink
{
    [JsonPropertyName("rel")]
    public string Rel { get; set; } = null!;

    [JsonPropertyName("type")]
    public string? LinkType { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("template")]
    public string? Template { get; set; }
}