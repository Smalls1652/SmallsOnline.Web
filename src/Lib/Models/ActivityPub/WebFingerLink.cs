namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Contains data for a WebFinger link.
/// </summary>
public class WebFingerLink : IWebFingerLink
{
    /// <inheritdoc />
    [JsonPropertyName("rel")]
    public string Rel { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string? LinkType { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("template")]
    public string? Template { get; set; }
}