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

    [Parameter]
    public NavigationManager? NavigationManager { get; set; }

    private string? _contentHtml;

    protected override void OnParametersSet()
    {
        if (NavigationManager is not null)
        {
            _contentHtml = FixFootnoteLinks();
        }
        else
        {
            _contentHtml = BlogEntry.ContentHtml;
        }

        base.OnParametersSet();
    }

    private string FixFootnoteLinks()
    {
        if (BlogEntry is not null && BlogEntry.ContentHtml is not null)
        {
            Regex footnoteLinkRegex =
                new(
                    "<a (?'idProperty'id=\"(?'id'(?:fnref|fn):.+?)\" |)href=\"(?'footnoteAnchorTag'#(?:fnref|fn):.+?\") class=\"(?'class'.+?)\">");

            string modifiedEntryContent = BlogEntry.ContentHtml;

            Match footnoteLinkMatch = footnoteLinkRegex.Match(BlogEntry.ContentHtml);

            while (footnoteLinkMatch.Success)
            {
                ComponentLogger.LogInformation("Fixing footnote link: '{text}'", footnoteLinkMatch.Value);
                if (footnoteLinkMatch.Groups["idProperty"] is not null &&
                    footnoteLinkMatch.Groups["footnoteAnchorTag"] is not null)
                {
                    modifiedEntryContent = modifiedEntryContent.Replace(
                        oldValue: footnoteLinkMatch.Value,
                        newValue:
                        $"<a id=\"{footnoteLinkMatch.Groups["id"].Value}\" href=\"{NavigationManager.Uri}{footnoteLinkMatch.Groups["footnoteAnchorTag"].Value}\" class=\"{footnoteLinkMatch.Groups["class"].Value}\">"
                    );
                }
                else
                {
                    modifiedEntryContent = modifiedEntryContent.Replace(
                        oldValue: footnoteLinkMatch.Value,
                        newValue:
                        $"<a href=\"{NavigationManager.Uri}{footnoteLinkMatch.Groups["footnoteAnchorTag"].Value}\" class=\"{footnoteLinkMatch.Groups["class"].Value}\">"
                    );
                }

                footnoteLinkMatch = footnoteLinkMatch.NextMatch();
            }

            return modifiedEntryContent;
        }

        throw new NullReferenceException("Content was null.");
    }
}