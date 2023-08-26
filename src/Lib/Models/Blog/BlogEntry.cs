using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using SmallsOnline.Web.Lib.Models.Database;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SmallsOnline.Web.Lib.Models.Blog;

/// <summary>
/// Contains the data for a blog post.
/// </summary>
public class BlogEntry : DatabaseItem, IBlogEntry
{
    [JsonConstructor()]
    public BlogEntry()
    { }

    /// <summary>
    /// Instantiates a new blog entry from a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="title">The title for the blog entry.</param>
    /// <param name="postedDate">The datetime for the blog entry.</param>
    /// <param name="tags">A collection of tags for the blog entry.</param>
    /// <param name="urlId">A unique URL id for the blog entry.</param>
    /// <exception cref="FileNotFoundException">Thrown whenever the provided file path could not be resolved.</exception>
    /// <exception cref="Exception">Thrown whenever the provided file does not have the correct file extension.</exception>
    public BlogEntry(string filePath, string? title, DateTimeOffset? postedDate, List<string> tags, string? urlId)
    {
        // Get the full path to the file.
        string absoluteFilePath = Path.GetFullPath(filePath);

        // Check if the file exists.
        bool fileExists = File.Exists(absoluteFilePath);
        if (!fileExists)
        {
            // Throw an exception if the file does not exist.
            throw new FileNotFoundException($"The file at the path '{filePath}' does not exist.");
        }

        // Get information about the file,
        // and check if the file extension is correct.
        FileInfo fileInfo = new(absoluteFilePath);
        if (fileInfo.Extension != ".md" && fileInfo.Extension != ".txt")
        {
            // Throw an exception if the file extension is incorrect.
            throw new Exception("The provided file does not have an extension of '.md' or '.txt'.");
        }

        // Get the file contents.
        using (StreamReader streamReader = new(absoluteFilePath))
        {
            Content = streamReader.ReadToEnd();
        }

        // Set the properties to the provided values.
        Id = Guid.NewGuid().ToString();
        _urlId = urlId;
        Title = title;
        PostedDate = postedDate;
        Tags = tags;
    }

    /// <summary>
    /// Instantiates a new blog entry from provided content.
    /// </summary>
    /// <param name="title">The title for the blog entry.</param>
    /// <param name="postedDate">The datetime for the blog entry.</param>
    /// <param name="content"></param>
    /// <param name="tags">A collection of tags for the blog entry.</param>
    /// <param name="urlId">A unique URL id for the blog entry.</param>
    public BlogEntry(string? title, DateTimeOffset? postedDate, string? content, List<string>? tags, string? urlId)
    {
        Id = Guid.NewGuid().ToString();
        _urlId = urlId;
        Title = title;
        PostedDate = postedDate;
        Content = content;
        Tags = tags;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    [JsonPropertyName("blogTitle")]
    public string? Title { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("blogPostedDate")]
    public DateTimeOffset? PostedDate { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("blogContent")]
    public string? Content { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("blogTags")]
    public IEnumerable<string>? Tags { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("blogIsPublished")]
    public bool IsPublished { get; set; }

    /// <inheritdoc />
    [JsonIgnore()]
    public string? ContentHtml
    {
        get
        {
            return (Content is not null) switch
            {
                true => Markdown.ToHtml(
                    markdown: Content!,
                    pipeline: new MarkdownPipelineBuilder()
                        .UseGenericAttributes()
                        .UsePipeTables()
                        .UseFootnotes()
                        .UseBootstrap()
                        .UseAutoLinks()
                        .Build()
                ),
                _ => null
            };
        }
    }

    private string? _urlId;

    /// <summary>
    /// Convert to JSON.
    /// </summary>
    /// <returns>A JSON representation of the <see cref="BlogEntry" /> object.</returns>
    public string ConvertToJson() => JsonSerializer.Serialize(this);

    /// <summary>
    /// Converts a JSON string to a <see cref="BlogEntry" /> object.
    /// </summary>
    /// <param name="jsonContent">The JSON string to convert from.</param>
    /// <returns>A <see cref="BlogEntry" /> object.</returns>
    public static BlogEntry? ConvertFromJson(string jsonContent) => JsonSerializer.Deserialize<BlogEntry>(jsonContent);

    /// <summary>
    /// Convert Markdown content to a <see cref="BlogEntry" /> object.
    /// </summary>
    /// <param name="content">The Markdown content to convert from.</param>
    /// <returns>A <see cref="BlogEntry" /> object.</returns>
    /// <exception cref="Exception">Thrown whenever the provided content isn't in the correct format.</exception>
    public static BlogEntry ConvertFromMarkdown(string content)
    {
        Regex contentRegex = new(@"(?:-{3}|\.{3})\r\n(?'metadata'.+?)\r\n(?:-{3}|\.{3})\r\n(?'content'.+)",
            RegexOptions.Singleline);
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

    /// <summary>
    /// Get an excerpt from the blog entry.
    /// </summary>
    /// <param name="asPlainText">Whether to generate as plain text or not.</param>
    /// <returns>An excerpt of the blog entry.</returns>
    /// <exception cref="NullReferenceException">Thrown when the <see cref="Content" /> property is null.</exception>
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