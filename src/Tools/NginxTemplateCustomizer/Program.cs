using System.Diagnostics;

CancellationTokenSource cts = new();
CancellationToken cancellationToken = cts.Token;

string nginxConfigPath = "/etc/nginx/http.d/default.conf";

string serverPort = Environment.GetEnvironmentVariable("SERVER_PORT") ?? "80";
string proxyPassUri = Environment.GetEnvironmentVariable("PROXY_PASS_URI") ?? "http://localhost:5000";
bool enableLogging = Environment.GetEnvironmentVariable("ENABLE_LOGGING") is not null ? bool.Parse(Environment.GetEnvironmentVariable("ENABLE_LOGGING")!) : false;

Console.WriteLine($"Server port: {serverPort}");
Console.WriteLine($"Proxy pass URI: {proxyPassUri}");
Console.WriteLine($"Logging enabled: {enableLogging}");

string nginxConfig = await File.ReadAllTextAsync(nginxConfigPath);

nginxConfig = nginxConfig
    .Replace("{{ SERVER_PORT }}", serverPort)
    .Replace("{{ PROXY_PASS_URI }}", proxyPassUri)
    .Replace("{{ LOGGING }}", enableLogging ? "/dev/stdout main" : "off");

Console.WriteLine("Writing updated Nginx config file...");

FileStream nginxConfigFileStream = File.Open(
    path: nginxConfigPath,
    mode: FileMode.Truncate
);
using StreamWriter nginxConfigWriter = new(nginxConfigFileStream);

nginxConfigWriter.Write(nginxConfig);
nginxConfigWriter.Close();

Console.WriteLine("Nginx config file updated successfully!");

Console.WriteLine("Starting Nginx...");

ProcessStartInfo nginxStartInfo = new()
{
    FileName = "nginx",
    ArgumentList = {
        "-g",
        """daemon off;"""
    },
    UseShellExecute = false,
    CreateNoWindow = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};

Process? nginxProcess = Process.Start(nginxStartInfo);

if (nginxProcess is null)
{
    Console.WriteLine("Failed to start Nginx!");
    return 1;
}

using StreamReader nginxOutputReader = nginxProcess.StandardOutput;
using StreamReader nginxErrorReader = nginxProcess.StandardError;

Console.WriteLine("Nginx started successfully!");

while (!cancellationToken.IsCancellationRequested && !nginxProcess.HasExited)
{
    if (cancellationToken.IsCancellationRequested)
    {
        nginxProcess.Kill();
    }

    string? nginxOutput = await nginxOutputReader.ReadLineAsync();
    string? nginxError = await nginxErrorReader.ReadLineAsync();

    if (nginxOutput is not null)
    {
        Console.WriteLine(nginxOutput);
    }

    if (nginxError is not null)
    {
        Console.WriteLine(nginxError);
    }
}

Console.WriteLine("Nginx stopped!");

return nginxProcess.ExitCode;
//await nginxProcess.WaitForExitAsync(cancellationToken);
