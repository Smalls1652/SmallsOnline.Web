namespace SmallsOnline.Web.Lib.Models.ActivityPub;

/// <summary>
/// Contains the data for a WebFinger response.
/// </summary>
public class WebFingerResponse : IWebFingerResponse
{
    /// <inheritdoc />
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("aliases")]
    public string[] Aliases { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("links")]
    public WebFingerLink[] WebFingerLinks { get; set; } = null!;
}