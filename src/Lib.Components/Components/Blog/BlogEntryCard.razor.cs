using System.Text.RegularExpressions;
using SmallsOnline.Web.Lib.Models.Blog;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace SmallsOnline.Web.Lib.Components.Blog;

/// <summary>
/// Component for displaying a blog post.
/// </summary>
public partial class BlogEntryCard : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Logger for the component.
    /// </summary>
    [Inject]
    protected ILogger<BlogEntryCard> ComponentLogger { get; set; } = null!;

    /// <summary>
    /// Data for a blog entry.
    /// </summary>
    [Parameter, EditorRequired]
    public BlogEntry BlogEntry { get; set; } = null!;

    /// <summary>
    /// Optional <see cref="NavigationManager"/> to use for modifying links in the blog entry.
    /// </summary>
    [Parameter]
    public NavigationManager? InputNavigationManager { get; set; }

    private string? _contentHtml;

    protected override void OnParametersSet()
    {
        _contentHtml = FixFootnoteLinks();

        base.OnParametersSet();
    }

    /// <summary>
    /// Fix footnote links in the blog entry to use the current page's URL.
    /// </summary>
    /// <remarks>This has to be done due to how the <see cref="Markdig" /> library generates links for footnotes.</remarks>
    /// <returns>An updated string value of the HTML.</returns>
    /// <exception cref="NullReferenceException">Thrown when the content of the blog entry is null.</exception>
    private string FixFootnoteLinks()
    {
        // If the content and the HTML content of the blog entry is not null,
        // then modify the footnote links in the HTML content.
        if (BlogEntry.ContentHtml is not null)
        {

            // Instantiate a string of the current HTML content of the blog entry,
            // so it can be used to hold the updated HTML content.
            string modifiedEntryContent = BlogEntry.ContentHtml;

            // Find the first match of the footnote link regex pattern in the HTML content.
            Match footnoteLinkMatch = FootnoteLinkRegex().Match(BlogEntry.ContentHtml);

            // Loop while there are still successful matches.
            while (footnoteLinkMatch.Success)
            {
                ComponentLogger.LogInformation("Fixing footnote link: '{text}'", footnoteLinkMatch.Value);

                modifiedEntryContent = modifiedEntryContent.Replace(
                    oldValue: footnoteLinkMatch.Value,
                    newValue: $"href=\"{NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}{footnoteLinkMatch.Groups["href"].Value}\""
                );

                // Find the next match.
                footnoteLinkMatch = footnoteLinkMatch.NextMatch();
            }

            // Return the updated HTML content.
            return modifiedEntryContent;
        }

        // If the content or the HTML content of the blog entry is null,
        // then throw a null reference exception.
        throw new NullReferenceException("Content was null.");
    }

    [GeneratedRegex(
        pattern: @"href=""(?'href'#.+?)"""
    )]
    private static partial Regex FootnoteLinkRegex();
}