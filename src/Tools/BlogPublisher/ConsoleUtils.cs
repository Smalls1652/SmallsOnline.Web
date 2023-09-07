namespace SmallsOnline.Web.Tools.BlogPublisher;

/// <summary>
/// Helper utilities for 
/// </summary>
public static class ConsoleUtils
{
    public static async Task WriteOutAsync(string message)
    {
        await Console.Out.WriteLineAsync(message);
    }

    public static async Task WriteErrorAsync(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Out.WriteLineAsync(message);
        Console.ResetColor();
    }

    public static async Task WriteStackTraceAsync(Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        await Console.Error.WriteLineAsync(exception.StackTrace);
        Console.ResetColor();
    }
}