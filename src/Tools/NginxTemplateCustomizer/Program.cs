string nginxConfigPath = "/etc/nginx/http.d/default.conf";

string serverPort = Environment.GetEnvironmentVariable("SERVER_PORT") ?? "80";
string proxyPassUri = Environment.GetEnvironmentVariable("PROXY_PASS_URI") ?? "http://localhost:5000";

Console.WriteLine($"Server port: {serverPort}");
Console.WriteLine($"Proxy pass URI: {proxyPassUri}");

string nginxConfig = await File.ReadAllTextAsync(nginxConfigPath);

nginxConfig = nginxConfig
    .Replace("{{ SERVER_PORT }}", serverPort)
    .Replace("{{ PROXY_PASS_URI }}", proxyPassUri);

Console.WriteLine("Writing updated Nginx config file...");

FileStream nginxConfigFileStream = File.Open(
    path: nginxConfigPath,
    mode: FileMode.Truncate
);
using StreamWriter nginxConfigWriter = new(nginxConfigFileStream);

nginxConfigWriter.Write(nginxConfig);
nginxConfigWriter.Close();