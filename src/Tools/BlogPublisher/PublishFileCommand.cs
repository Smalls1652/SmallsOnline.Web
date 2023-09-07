using System.CommandLine;
using SmallsOnline.Web.Tools.BlogPublisher.Actions;

namespace SmallsOnline.Web.Tools.BlogPublisher;

/// <summary>
/// CLI command for publishing a single file to the blog.
/// </summary>
public class PublishFileCommand : CliCommand
{
    public PublishFileCommand() : base("publish")
    {
        Description = "Publish a single file to the blog.";

        Options.Add(new CliOption<string>("-i")
        {
            Description = "The markdown file to be published.",
            Aliases = { "--input-file" },
            Required = true
        });

        Action = new PublishFileAction();
    }
}