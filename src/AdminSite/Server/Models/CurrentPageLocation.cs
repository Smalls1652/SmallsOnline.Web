using System.Text.RegularExpressions;

namespace SmallsOnline.Web.AdminSite.Server.Models;

/// <summary>
/// Holds data for the current page location.
/// </summary>
public partial class CurrentPageLocation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentPageLocation"/> class.
    /// </summary>
    /// <param name="inputUri">The URI for the current location.</param>
    public CurrentPageLocation(string inputUri)
    {
        Match uriSectionMatch = UriSectionRegex().Match(inputUri);

        if (uriSectionMatch.Success == false)
        {
            throw new($"Failed to parse the current page to update the navigation bar. Uri provided: {inputUri}");
        }
        else
        {
            HostName = uriSectionMatch.Groups["hostName"].Value;
            Path = uriSectionMatch.Groups["path"].Value;
            TopLevelPage = uriSectionMatch.Groups["topLevelPage"].Value;
            SecondaryPages = uriSectionMatch.Groups["secondaryPages"].Value;
        }
    }

    /// <summary>
    /// The host name for the current page.
    /// </summary>
    public string HostName { get; set; } = null!;

    /// <summary>
    /// The path for the current page.
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// The top level page for the current page.
    /// </summary>
    public string? TopLevelPage { get; set; }

    /// <summary>
    /// The secondary pages for the current page.
    /// </summary>
    public string? SecondaryPages { get; set; }

    [GeneratedRegex(
        pattern: "^(?:https|http)://(?'hostName'.+?)(?'path'/(?'topLevelPage'.*?)(?'secondaryPages'/.*?|))(?:#.*|)$",
        options: RegexOptions.Multiline
    )]
    private static partial Regex UriSectionRegex();
}