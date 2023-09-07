using System.CommandLine;
using SmallsOnline.Web.Tools.BlogPublisher;

int returnCode = 0;

BlogPublisherRootCommand rootCommand = new();

CliConfiguration cliConfiguration = new(rootCommand);

await cliConfiguration.InvokeAsync(args);

return returnCode;