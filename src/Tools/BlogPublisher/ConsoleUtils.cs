namespace SmallsOnline.Web.Tools.BlogPublisher;

/// <summary>
/// Helper utilities for the console.
/// </summary>
public static class ConsoleUtils
{
    /// <summary>
    /// Writes a message to standard out (stdout) asynchronously.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <returns></returns>
    public static async Task WriteOutAsync(string message)
    {
        await Console.Out.WriteLineAsync(message);
    }

    /// <summary>
    /// Write an error message to standard out (stdout) asynchronously.
    /// </summary>
    /// <param name="message">The error message to write.</param>
    /// <returns></returns>
    public static async Task WriteErrorAsync(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Out.WriteLineAsync(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Write a stack trace to standard error (stderr) asynchronously.
    /// </summary>
    /// <param name="exception">The exception to write.</param>
    /// <returns></returns>
    public static async Task WriteStackTraceAsync(Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        await Console.Error.WriteLineAsync(exception.StackTrace);
        Console.ResetColor();
    }
}