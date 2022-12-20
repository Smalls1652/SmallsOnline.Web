using Microsoft.AspNetCore.Mvc;
using SmallsOnline.Web.Lib.Models.ActivityPub;

namespace SmallsOnline.Web.Api.Controllers;

[ApiController]
[Route("/api/activitypub")]
public class ActivityPubController : ControllerBase
{
    private readonly ILogger<ActivityPubController> _logger;
    private readonly ICosmosDbService _cosmosDbService;

    public ActivityPubController(ILogger<ActivityPubController> logger, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
    }

    [HttpGet("webfinger", Name = "GetWebFingerResponse")]
    public async Task<string> GetWebFingerResponse()
    {
        WebFingerResponse retrievedWebFingerData = await _cosmosDbService.GetWebFingerResponseAsync();

        string jsonResponse = JsonSerializer.Serialize<WebFingerResponse>(
            value: retrievedWebFingerData,
            options: new JsonSerializerOptions
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }
        );

        return jsonResponse;
    }
}