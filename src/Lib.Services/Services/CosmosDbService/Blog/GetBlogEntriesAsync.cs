using System.Text;
using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Models.CosmosDB;

namespace SmallsOnline.Web.Lib.Services;

public partial class CosmosDbService : ICosmosDbService
{
    /// <inheritdoc />
    /// <exception cref="Exception"></exception>
    public async Task<BlogEntry[]> GetBlogEntriesAsync(int pageNumber = 1)
    {
        // Set the offset count for the Cosmos DB SQL query.
        int offsetNum = pageNumber switch
        {
            0 => throw new Exception("Invalid page number."), // Page number cannot be zero.
            1 => 0, // Offset always starts at 0 for the first page.
            _ => (pageNumber - 1) * 5 // Offset starts at the number of items per page * the page number.
        };

        // Get the container where the blog entries are stored.
        Container container = _cosmosDbClient.GetContainer(_containerName, "blogs");

        // Build the Cosmos DB SQL query.
        string coreQuery = $"WHERE c.partitionKey = \"blog-entry\" AND c.blogIsPublished = true ORDER BY c.blogPostedDate DESC OFFSET {offsetNum} LIMIT 5";
        QueryDefinition resultsQuery = new($"SELECT c.id, c.partitionKey, c.blogUrlId, c.blogTitle, c.blogPostedDate, c.blogContent, c.blogTags, c.blogIsPublished FROM c {coreQuery}");

        // Initialize a list to hold the blog entries.
        BlogEntry[]? blogEntries = null;

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: resultsQuery,
            requestOptions: new()
            {
                PartitionKey = new("blog-entry")
            }
        );

        // Execute the Cosmos DB SQL query and get the results.
        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();

            CosmosDbResponse<BlogEntry>? blogEntriesResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: response.Content,
                jsonTypeInfo: CoreJsonContext.Default.CosmosDbResponseBlogEntry
            );

            if (blogEntries is null)
            {
                blogEntries = new BlogEntry[blogEntriesResponse!.Count];
            }

            int i = 0;
            // Loop through each database entry retrieved.
            foreach (BlogEntry item in blogEntriesResponse!.Documents!)
            {
                // If the content of the blog post is not null,
                // then attempt to shorten the content.
                if (item.Content is not null)
                {
                    // Initialize a StringBuilder to hold the shortened content.
                    StringBuilder markdownShort = new();

                    // Use StringReader to read the content.
                    using (StringReader stringReader = new(item.Content))
                    {
                        // Loop through each line until 'moreLineFound' is set to true.
                        bool moreLineFound = false;
                        while (!moreLineFound)
                        {
                            string? line = await stringReader.ReadLineAsync();

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

                    // Set the content property to the shortened content.
                    item.Content = markdownShort.ToString();
                }

                // Add the blog entry to the list.
                blogEntries[i] = item;
                i++;
            }
        }

        return blogEntries!;
    }
}