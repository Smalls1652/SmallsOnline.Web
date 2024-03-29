using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

/// <summary>
/// Error page for the site.
/// </summary>
/// <remarks>
/// Might be replaced with a built-in Blazor error page in the future.
/// </remarks>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class Error : PageModel
{
    /// <summary>
    /// The request ID.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// The original status code.
    /// </summary>
    public int OriginalStatusCode { get; set; }

    /// <summary>
    /// Enriched status code message from <see cref="OriginalStatusCode"/>.
    /// </summary>
    public string StatusCodeMessage => (HttpStatusCode)OriginalStatusCode switch
    {
        HttpStatusCode.BadRequest => "Bad Request",
        HttpStatusCode.Unauthorized => "Unauthorized",
        HttpStatusCode.Forbidden => "Forbidden",
        HttpStatusCode.NotFound => "Not Found",
        HttpStatusCode.MethodNotAllowed => "Method Not Allowed",
        HttpStatusCode.NotAcceptable => "Not Acceptable",
        HttpStatusCode.ProxyAuthenticationRequired => "Proxy Authentication Required",
        HttpStatusCode.RequestTimeout => "Request Timeout",
        HttpStatusCode.Conflict => "Conflict",
        HttpStatusCode.Gone => "Gone",
        HttpStatusCode.LengthRequired => "Length Required",
        HttpStatusCode.PreconditionFailed => "Precondition Failed",
        HttpStatusCode.RequestEntityTooLarge => "Request Entity Too Large",
        HttpStatusCode.RequestUriTooLong => "Request Uri Too Long",
        HttpStatusCode.UnsupportedMediaType => "Unsupported Media Type",
        HttpStatusCode.RequestedRangeNotSatisfiable => "Requested Range Not Satisfiable",
        HttpStatusCode.ExpectationFailed => "Expectation Failed",
        HttpStatusCode.MisdirectedRequest => "Misdirected Request",
        HttpStatusCode.UnprocessableEntity => "Unprocessable Entity",
        HttpStatusCode.Locked => "Locked",
        HttpStatusCode.FailedDependency => "Failed Dependency",
        HttpStatusCode.UpgradeRequired => "Upgrade Required",
        HttpStatusCode.PreconditionRequired => "Precondition Required",
        HttpStatusCode.TooManyRequests => "Too Many Requests",
        HttpStatusCode.RequestHeaderFieldsTooLarge => "Request Header Fields Too Large",
        HttpStatusCode.UnavailableForLegalReasons => "Unavailable For Legal Reasons",
        HttpStatusCode.InternalServerError => "Internal Server Error",
        HttpStatusCode.NotImplemented => "Not Implemented",
        HttpStatusCode.BadGateway => "Bad Gateway",
        HttpStatusCode.ServiceUnavailable => "Service Unavailable",
        HttpStatusCode.GatewayTimeout => "Gateway Timeout",
        HttpStatusCode.HttpVersionNotSupported => "Http Version Not Supported",
        HttpStatusCode.VariantAlsoNegotiates => "Variant Also Negotiates",
        HttpStatusCode.InsufficientStorage => "Insufficient Storage",
        HttpStatusCode.LoopDetected => "Loop Detected",
        HttpStatusCode.NotExtended => "Not Extended",
        HttpStatusCode.NetworkAuthenticationRequired => "Network Authentication Required",
        _ => "Unknown Status Code"
    };

    /// <summary>
    /// The original path the user was trying to access.
    /// </summary>
    public string? OriginalPath { get; set; }

    /// <summary>
    /// Whether to show the request ID or not.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<Error> _logger;

    public Error(ILogger<Error> logger)
    {
        _logger = logger;
    }

    public void OnGet(int statusCode)
    {
        OriginalStatusCode = statusCode;

        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var statusCodeReExecuteFeature =
            HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        if (statusCodeReExecuteFeature is not null)
        {
            OriginalPath = string.Join(statusCodeReExecuteFeature.OriginalPathBase, statusCodeReExecuteFeature.OriginalPath, statusCodeReExecuteFeature.OriginalQueryString);
        }
    }
}