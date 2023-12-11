using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using SmallsOnline.Web.Lib.Models.Blog;
using SmallsOnline.Web.Lib.Services;

namespace SmallsOnline.Web.Tools.BlogPublisher.Actions;

/// <summary>
/// <see cref="CliAction">CliAction</see> implementation for publishing a file.
/// </summary>
public class PublishFileAction : AsynchronousCliAction
{
    public PublishFileAction(bool terminating = true)
    {
        Terminating = terminating;
    }

    /// <summary>
    /// The action to run when the command is invoked.
    /// </summary>
    /// <param name="parseResult"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {;
        string? websiteUrl = Environment.GetEnvironmentVariable("WEBSITE_URL");

        string inputFile = parseResult.GetValue<string>("-i")!;

        // Attempt to get the CosmosDB name from either the provided option or the environment variable.
        string? cosmosDbName = parseResult.GetValue<string?>("--cosmosdb-name") ?? Environment.GetEnvironmentVariable("COSMOSDB_NAME");
        if (cosmosDbName is null)
        {
            await ConsoleUtils.WriteErrorAsync("No CosmosDB name was provided.");
            return 40;
        }

        // Attempt to get the CosmosDB connection string from either the provided option or the environment variable.
        string? cosmosDbConnectionString = parseResult.GetValue<string?>("--cosmosdb-connection-string") ?? Environment.GetEnvironmentVariable("COSMOSDB_CONNECTION_STRING");
        if (cosmosDbConnectionString is null)
        {
            await ConsoleUtils.WriteErrorAsync("No CosmosDB connection string was provided.");
            return 40;
        }

        string? storageAccountDomainName = parseResult.GetValue<string?>("--storage-account-domain-name") ?? Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_DOMAIN_NAME");
        if (storageAccountDomainName is null)
        {
            await ConsoleUtils.WriteErrorAsync("No storage account domain name was provided.");
            return 40;
        }

        string? storageAccountConnectionString = parseResult.GetValue<string?>("--storage-account-connection-string") ?? Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING");
        if (storageAccountConnectionString is null)
        {
            await ConsoleUtils.WriteErrorAsync("No storage account connection string was provided.");
            return 40;
        }

        await Console.Out.WriteAsync("\nConnecting to database... ");

        CosmosDbService? cosmosDbService;
        try
        {
            cosmosDbService = new(cosmosDbConnectionString, cosmosDbName);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An error occurred while trying to connect to the database: {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 45;
        }

        await Console.Out.WriteAsync("\nConnecting to storage account... ");
        BlobStorageService blobStorageService = new(storageAccountConnectionString, storageAccountDomainName);

        Console.ForegroundColor = ConsoleColor.Green;
        await Console.Out.WriteAsync("Connected!");
        Console.ResetColor();

        await Console.Out.WriteAsync("\nGetting file content... ");

        string absoluteFilePath = Path.GetFullPath(inputFile);

        string? fileContent;
        try
        {
            if (!File.Exists(absoluteFilePath))
            {
                throw new FileNotFoundException($"The file at the path '{inputFile}' does not exist.");
            }

            FileInfo fileInfo = new(absoluteFilePath);
            if (fileInfo.Extension != ".md" && fileInfo.Extension != ".txt")
            {
                throw new IOException("The provided file does not have an extension of '.md' or '.txt'.");
            }

            using StreamReader streamReader = new(absoluteFilePath);
            fileContent = await streamReader.ReadToEndAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(fileContent))
            {
                throw new NullReferenceException("The provided file is empty.");
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An error occurred while trying to find the file: {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 41;
        }
        catch (IOException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An error occurred while trying open the file: {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 42;
        }
        catch (NullReferenceException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An error occurred with the file: {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 43;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An unhandled error occurred: {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 44;
        }

        fileContent = await ImageSyntaxParser.ReplaceAndUploadImagesAsync(fileContent, absoluteFilePath, blobStorageService);

        BlogEntry? blogEntry;
        try
        {
            blogEntry = BlogEntry.ConvertFromMarkdown(fileContent);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An error occurred while trying to convert the file {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 45;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        await Console.Out.WriteAsync("Done!\n\n");
        Console.ResetColor();

        await Console.Out.WriteLineAsync("Blog entry details:");

        // Generate a table, in the style of a markdown table, to display the blog entry details.
        // Was this really necessary? No. But it's cool.
        StringBuilder tableStringBuiler = new();
        tableStringBuiler.Append("|");
        tableStringBuiler.Append(" ID");
        tableStringBuiler.Append(' ', blogEntry.Id.Length - 2);
        tableStringBuiler.Append(" | Title");
        tableStringBuiler.Append(' ', blogEntry.Title!.Length - 5);
        tableStringBuiler.Append(" |\n");
        tableStringBuiler.Append("| ");
        tableStringBuiler.Append('-', blogEntry.Id.Length);
        tableStringBuiler.Append(" | ");
        tableStringBuiler.Append('-', blogEntry.Title!.Length);
        tableStringBuiler.Append(" |\n");
        tableStringBuiler.AppendLine($"| {blogEntry.Id} | {blogEntry.Title} |\n");
        await Console.Out.WriteLineAsync(tableStringBuiler.ToString());

        try
        {
            await cosmosDbService.AddOrUpdateBlogEntryAsync(blogEntry);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("Failed!");
            Console.ResetColor();

            await ConsoleUtils.WriteErrorAsync($"\n[{ex.GetType()}] An error occurred while trying to write to the database: {ex.Message}");
            await ConsoleUtils.WriteStackTraceAsync(ex);
            return 46;
        }

        await Console.Out.WriteLineAsync("Blog file published!");

        if (websiteUrl is not null)
        {
            await Console.Out.WriteLineAsync($"\nView it here: ${websiteUrl}/blog/entry/{blogEntry.UrlId}");
        }

        return 0;
    }
}