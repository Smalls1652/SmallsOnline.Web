using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmallsOnline.Web.Lib.Models.ActivityPub;

namespace SmallsOnline.Web.PublicSite.Server.Pages;

public class WebFinger : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public WebFinger(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public WebFingerResponse? WebFingerResponse { get; set; }
    public string? WebFingerJsonString { get; set; }

    public async Task OnGet()
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient("PublicApi");
        WebFingerResponse = await httpClient.GetFromJsonAsync<WebFingerResponse?>("api/activitypub/webfinger");

        if (WebFingerResponse is not null)
        {
            WebFingerJsonString = JsonSerializer.Serialize(
                value: WebFingerResponse,
                options: new()
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }
            );
        }
    }
}