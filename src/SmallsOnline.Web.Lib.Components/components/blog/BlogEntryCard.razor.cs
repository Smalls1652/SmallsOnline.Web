using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.Lib.Components.Blog;

/// <summary>
/// Component for displaying a blog post.
/// </summary>
public partial class BlogEntryCard : ComponentBase
{
    /// <summary>
    /// Data for a blog entry.
    /// </summary>
    [Parameter, EditorRequired]
    public BlogEntry BlogEntry { get; set; } = null!;
}