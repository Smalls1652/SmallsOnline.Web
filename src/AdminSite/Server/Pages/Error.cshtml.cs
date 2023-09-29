using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmallsOnline.Web.AdminSite.Server.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class Error : PageModel
{
    public string? RequestId { get; set; }

    public int OriginalStatusCode { get; set; }

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

    public string? OriginalPath { get; set; }

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