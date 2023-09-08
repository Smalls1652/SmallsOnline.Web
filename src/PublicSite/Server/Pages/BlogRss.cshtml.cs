using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

public class BlogRss : PageModel
{
    private readonly ICosmosDbService _cosmosDbService;

    public BlogRss(ICosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public string? RssFeedContent { get; set; }

    public async Task OnGetAsync()
    {
        MemoryStream memoryStream = new();

        SyndicationFeed syndicationFeed = new(
            title: "Smalls.Online Blog",
            description: "The blog for Smalls.Online.",
            feedAlternateLink: new Uri("https://smalls.online/blog/rss")
        )
        {
            Copyright = new("Copyright \u00a9 2023 Tim Small"),
            Language = "en-US",
            LastUpdatedTime = DateTimeOffset.Now
        };

        syndicationFeed.Authors.Add(
            new()
            {
                Name = "Tim Small",
                Uri = "https://smalls.online"
            }
        );

        int totalBlogPages = await _cosmosDbService.GetBlogTotalPagesAsync();
        List<SyndicationItem> entryItems = new();

        for (int i = 1; i <= totalBlogPages; i++)
        {
            BlogEntry[] blogEntries = await _cosmosDbService.GetBlogEntriesAsync(i);

            foreach (BlogEntry blogEntry in blogEntries)
            {
                SyndicationItem syndicationItem = new()
                {
                    Title = new(blogEntry.Title),
                    PublishDate = blogEntry.PostedDate!.Value,
                    Id = blogEntry.Id,
                    Links = { new(new($"https://smalls.online/blog/entry/{blogEntry.UrlId}")) },
                    Summary = new(blogEntry.GetExcerpt(true)),
                    Content = SyndicationContent.CreateHtmlContent(blogEntry.ContentHtml),
                    SourceFeed = syndicationFeed,
                    Categories = { new("blog") },
                    Authors =
                    {
                        new()
                        {
                            Name = "Tim Small",
                            Uri = "https://smalls.online"
                        }
                    }
                };

                entryItems.Add(syndicationItem);
            }
        }

        syndicationFeed.Items = entryItems;

        Rss20FeedFormatter formatter = new(syndicationFeed);

        await using XmlWriter xmlWriter = XmlWriter.Create(
            output: memoryStream,
            settings: new()
            {
                Encoding = Encoding.UTF8,
                Async = true,
                Indent = true
            }
        );

        formatter.WriteTo(xmlWriter);
        await xmlWriter.FlushAsync();

        using StreamReader streamReader = new(memoryStream);
        memoryStream.Position = 0;
        RssFeedContent = await streamReader.ReadToEndAsync();
        streamReader.Close();
    }
}