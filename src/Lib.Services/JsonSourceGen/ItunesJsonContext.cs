﻿using System.Text.Json.Serialization;
using SmallsOnline.Web.Lib.Models.Itunes;

namespace SmallsOnline.Web.Lib.Services;

/// <summary>
/// Source generated JSON context for the iTunes Search API classes.
/// </summary>
[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(ArtistItem))]
[JsonSerializable(typeof(ApiSearchResult<ArtistItem>))]
[JsonSerializable(typeof(SongItem))]
[JsonSerializable(typeof(ApiSearchResult<SongItem>))]
[JsonSerializable(typeof(AlbumItem))]
[JsonSerializable(typeof(ApiSearchResult<AlbumItem>))]
internal partial class ItunesJsonContext : JsonSerializerContext
{
}