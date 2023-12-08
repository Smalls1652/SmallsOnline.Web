namespace SmallsOnline.Web.Lib.Services;

public class OdesliServiceOptions
{
    /// <summary>
    /// The user agent to use when making HTTP requests.
    /// </summary>
    public string UserAgent { get; set; } = null!;

    /// <summary>
    /// Whether or not to enable logging.
    /// </summary>
    public bool EnableLogging { get; set; } = true;
}