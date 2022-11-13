using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using SmallsOnline.Web.Lib.Models.Database;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SmallsOnline.Web.Lib.Models.Blog;

public class BlogEntry : DatabaseItem, IBlogEntry
{
    [JsonConstructor()]
    public BlogEntry()
    { }

    public BlogEntry(string filePath, string? title, DateTimeOffset? postedDate, List<string> tags, string? urlId)
    {
        string absoluteFilePath = Path.GetFullPath(filePath);

        bool fileExists = File.Exists(absoluteFilePath);
        if (!fileExists)
        {
            throw new FileNotFoundException($"The file at the path '{filePath}' does not exist.");
        }

        FileInfo fileInfo = new(absoluteFilePath);
        if (fileInfo.Extension != ".md" && fileInfo.Extension != ".txt")
        {
            throw new Exception("The provided file does not have an extension of '.md' or '.txt'.");
        }

        using (StreamReader streamReader = new(absoluteFilePath))
        {
            Content = streamReader.ReadToEnd();
        }

        Id = Guid.NewGuid().ToString();
        _urlId = urlId;
        Title = title;
        PostedDate = postedDate;
        Tags = tags;
    }

    public BlogEntry(string? title, DateTimeOffset? postedDate, string? content, List<string>? tags, string? urlId)
    {
        Id = Guid.NewGuid().ToString();
        _urlId = urlId;
        Title = title;
        PostedDate = postedDate;
        Content = content;
        Tags = tags;
    }

    [JsonPropertyName("blogUrlId")]
    public string? UrlId 
    {
        get
        {
            if (_urlId is null)
            {
                return Id;
            }
            else
            {
                return _urlId;
            }
        }

        set => _urlId = value;
    }

    [JsonPropertyName("blogTitle")]
    public string? Title { get; set; }

    [JsonPropertyName("blogPostedDate")]
    public DateTimeOffset? PostedDate { get; set; }

    [JsonPropertyName("blogContent")]
    public string? Content { get; set; }

    [JsonPropertyName("blogTags")]
    public List<string>? Tags { get; set; }

    [JsonPropertyName("blogIsPublished")]
    public bool IsPublished { get; set; }

    [JsonIgnore()]
    public string? ContentHtml
    {
        get
        {
            if (Content is not null)
            {
                return Markdown.ToHtml(
                    markdown: Content,
                    pipeline: new MarkdownPipelineBuilder()
                        .UseGenericAttributes()
                        .UsePipeTables()
                        .UseBootstrap()
                        .UseAutoLinks()
                        .Build()
                );
            }
            else
            {
                return null;
            }
        }
    }

    private string? _urlId;

    public string ConvertToJson() => JsonSerializer.Serialize(this);

    public static BlogEntry? ConvertFromJson(string jsonContent) => JsonSerializer.Deserialize<BlogEntry>(jsonContent);

    public static BlogEntry ConvertFromMarkdown(string content)
    {
        Regex contentRegex = new(@"(?:-{3}|\.{3})\r\n(?'metadata'.+?)\r\n(?:-{3}|\.{3})\r\n(?'content'.+)", RegexOptions.Singleline);
        Match contentMatch = contentRegex.Match(content);

        if (!contentMatch.Success)
        {
            throw new Exception("The provided content is not in the correct format.");
        }
        else
        {
            StringReader metadataReader = new(contentMatch.Groups["metadata"].Value);
            IDeserializer yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            BlogEntry blogEntry = yamlDeserializer.Deserialize<BlogEntry>(metadataReader);
            blogEntry.Content = contentMatch.Groups["content"].Value;

            return blogEntry;
        }
    }

    public string GetExcerpt(bool asPlainText = false)
    {
        if (Content is not null)
        {
            // Intialize a StringBuilder to hold the shortened content.
            StringBuilder markdownShort = new();

            // Use StringReader to read the content.
            using (StringReader stringReader = new(Content))
            {
                // Loop through each line until 'moreLineFound' is set to true.
                bool moreLineFound = false;
                while (!moreLineFound)
                {
                    string? line = stringReader.ReadLine();

                    if (line == "<!--more-->")
                    {
                        // If the line is '<!--more-->',
                        // then set 'moreLineFound' to true and exit the loop.
                        moreLineFound = true;
                    }
                    else if (line is not null)
                    {
                        // If the line is not null and is not '<!--more-->',
                        // then add the line to the StringBuilder.
                        markdownShort.AppendLine(line);
                    }
                    else
                    {
                        // If we've reached the end of the content,
                        // then set 'moreLineFound' to true and exit the loop.
                        moreLineFound = true;
                        break;
                    }
                }
            }

            // Return the excerpt
            if (!asPlainText)
            {
                return markdownShort.ToString();
            }
            else
            {
                return Markdown.ToPlainText(
                    markdown: markdownShort.ToString(),
                    pipeline: new MarkdownPipelineBuilder()
                        .ConfigureNewLine(" ")
                        .Build()
                );
            }
        }
        else
        {
            throw new NullReferenceException("The 'Content' property is null.");
        }
    }
}