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
    public NavigationManager? NavigationManager { get; set; }

    private string? _contentHtml;

    protected override void OnParametersSet()
    {
        // Handle whether or not the NavigationManager parameter was set.
        if (NavigationManager is not null)
        {
            // If NavigationManager was set,
            // then we need to modify the footnote links in the blog entry and set '_contentHtml' to the modified value.
            _contentHtml = FixFootnoteLinks();
        }
        else
        {
            // If NavigationManager was not set,
            // then we can just set '_contentHtml' to the value of 'BlogEntry.ContentHtml'.
            _contentHtml = BlogEntry.ContentHtml;
        }

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
        if (BlogEntry is not null && BlogEntry.ContentHtml is not null)
        {
            // Regex pattern for finding footnote links in the HTML content.
            Regex footnoteLinkRegex =
                new(
                    "<a (?'idProperty'id=\"(?'id'(?:fnref|fn):.+?)\" |)href=\"(?'footnoteAnchorTag'#(?:fnref|fn):.+?\") class=\"(?'class'.+?)\">");

            // Instantiate a string of the current HTML content of the blog entry,
            // so it can be used to hold the updated HTML content.
            string modifiedEntryContent = BlogEntry.ContentHtml;

            // Find the first match of the footnote link regex pattern in the HTML content.
            Match footnoteLinkMatch = footnoteLinkRegex.Match(BlogEntry.ContentHtml);

            // Loop while there are still successful matches.
            while (footnoteLinkMatch.Success)
            {
                ComponentLogger.LogInformation("Fixing footnote link: '{text}'", footnoteLinkMatch.Value);

                // Logic for handling the different types of footnote links.
                if (footnoteLinkMatch.Groups["idProperty"] is not null &&
                    footnoteLinkMatch.Groups["footnoteAnchorTag"] is not null)
                {
                    // If the match has the 'idProperty' group,
                    // then replace the found match with an updated value that uses the current page's URL
                    // and includes the 'id' property.
                    modifiedEntryContent = modifiedEntryContent.Replace(
                        oldValue: footnoteLinkMatch.Value,
                        newValue:
                        $"<a id=\"{footnoteLinkMatch.Groups["id"].Value}\" href=\"{NavigationManager.Uri}{footnoteLinkMatch.Groups["footnoteAnchorTag"].Value}\" class=\"{footnoteLinkMatch.Groups["class"].Value}\">"
                    );
                }
                else
                {
                    // If the match does not have the 'idProperty' group,
                    // then replace the found match with an updated value that uses the current page's URL
                    // and does not include the 'id' property.
                    modifiedEntryContent = modifiedEntryContent.Replace(
                        oldValue: footnoteLinkMatch.Value,
                        newValue:
                        $"<a href=\"{NavigationManager.Uri}{footnoteLinkMatch.Groups["footnoteAnchorTag"].Value}\" class=\"{footnoteLinkMatch.Groups["class"].Value}\">"
                    );
                }

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
}