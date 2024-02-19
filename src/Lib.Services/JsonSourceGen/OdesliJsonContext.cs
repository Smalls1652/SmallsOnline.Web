using System.Text.Json.Serialization;
using SmallsOnline.Web.Lib.Models.Odesli;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Source generated JSON context for the Odesli API classes.
/// </summary>
[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(StreamingEntityItem))]
[JsonSerializable(typeof(PlatformEntityLink))]
[JsonSerializable(typeof(MusicEntityItem))]
[JsonSerializable(typeof(MusicEntityItem[]))]
[JsonSerializable(typeof(Dictionary<string, PlatformEntityLink>))]
[JsonSerializable(typeof(Dictionary<string, StreamingEntityItem>))]
internal partial class OdesliJsonContext : JsonSerializerContext
{
}