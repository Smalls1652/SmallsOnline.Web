using SmallsOnline.Web.Lib.Models.Blog;

namespace SmallsOnline.Web.Lib.Components.Blog;

public partial class BlogEntryCard : ComponentBase
{
    [Parameter, EditorRequired]
    public BlogEntry BlogEntry { get; set; } = null!;
}