using System.CommandLine;

namespace SmallsOnline.Web.Tools.BlogPublisher;

/// <summary>
/// <see cref="CliRootCommand" /> for the application.
/// </summary>
public class BlogPublisherRootCommand : CliRootCommand
{
    public BlogPublisherRootCommand() : base("Smalls.Online Blog Publisher")
    {
        Options.Add(new CliOption<string?>("--cosmosdb-name")
        {
            Description = "The CosmosDB database name that houses the containers.",
            Recursive = true
        });

        Options.Add(new CliOption<string?>("--cosmosdb-connection-string")
        {
            Description = "The connection string for authenticating to CosmosDB.",
            Recursive = true
        });

        Options.Add(new CliOption<string?>("--storage-account-domain-name")
        {
            Description = "The domain name for the storage account.",
            Recursive = true
        });

        Options.Add(new CliOption<string?>("--storage-account-connection-string")
        {
            Description = "The connection string for authenticating to the storage account.",
            Recursive = true
        });

        Add(new PublishFileCommand());
    }
}