using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using SmallsOnline.Web.Lib.Models.Database;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Albums;

/// <summary>
/// A form item for album data.
/// </summary>
public class AlbumDataFormItem : DatabaseItem, IAlbumData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumDataFormItem" /> class.
    /// </summary>
    public AlbumDataFormItem()
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "favorites-of-albums";
        SchemaVersion = "2.0";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumDataFormItem" /> class with the given list year.
    /// </summary>
    /// <param name="listYear">The list year for the item.</param>
    public AlbumDataFormItem(string? listYear)
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "favorites-of-albums";
        ListYear = listYear;
        SchemaVersion = "2.0";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumDataFormItem" /> class from the given <see cref="IAlbumData" /> object.
    /// </summary>
    /// <param name="albumData">The album data to import.</param>
    public AlbumDataFormItem(IAlbumData albumData)
    {
        Id = albumData.Id;
        PartitionKey = albumData.PartitionKey;
        SchemaVersion = albumData.SchemaVersion is null ? "2.0" : albumData.SchemaVersion;
        Title = albumData.Title;
        Artist = albumData.Artist;
        StandoutSongs = albumData.StandoutSongs;
        AlbumArtUrl = albumData.AlbumArtUrl;
        AlbumUrl = albumData.AlbumUrl;
        IsBest = albumData.IsBest;
        Comments = albumData.Comments;
        ReleaseDate = albumData.ReleaseDate;
        ListYear = albumData.ListYear;
    }

    /// <inheritdoc />
    [JsonPropertyName("@schemaVersion")]
    public string? SchemaVersion { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("albumTitle")]
    public string? Title { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("albumArtist")]
    public string? Artist { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("albumStandoutSongs")]
    public AlbumStandoutSongItem[]? StandoutSongs
    {
        get => _standoutSongs is null ? Array.Empty<AlbumStandoutSongItem>() : _standoutSongs?.ToArray();
        set => _standoutSongs = value is not null ? new(value) : null;
    }

    /// <inheritdoc />
    /// <remarks>Does not return when converted to JSON.</remarks>
    [JsonIgnore]
    public IEnumerable<AlbumStandoutSongItem>? OnlyStandoutSongs => GetOnlyStandoutSongs();

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("albumArtUrl")]
    public string? AlbumArtUrl { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("albumUrl")]
    public string? AlbumUrl { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("albumIsBest")]
    public bool IsBest { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("albumComments")]
    public string? Comments { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("albumReleaseDate")]
    public DateTimeOffset? ReleaseDate { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("listYear")]
    public string? ListYear { get; set; }

    /// <inheritdoc />
    /// <remarks>Does not return when converted to JSON.</remarks>
    [JsonIgnore]
    public string? AlbumId
    {
        get
        {
            // Instantiate a StringBuilder object.
            StringBuilder stringBuilder = new();

            // If the album artist is not null or empty,
            // format the text and append it to the string builder.
            if (Artist is not null)
            {
                stringBuilder.Append(BuildIdentifierText(Artist));
            }

            // If the album title is not null or empty,
            // format the text and append it to the string builder.
            if (Title is not null)
            {
                stringBuilder.Append("_");
                stringBuilder.Append(BuildIdentifierText(Title));
            }

            // Return the string builder's text and ensure it's lowercase.
            return stringBuilder.ToString().ToLower();
        }
    }

    private List<AlbumStandoutSongItem>? _standoutSongs;

    /// <summary>
    /// Add a standout song item to the album.
    /// </summary>
    /// <param name="standoutSong">The song to add.</param>
    public void AddStandoutSongItem(AlbumStandoutSongItem standoutSong)
    {
        if (_standoutSongs is null)
        {
            _standoutSongs = new();
        }

        _standoutSongs.Add(standoutSong);
    }

    /// <summary>
    /// Gets only the standout songs for the album.
    /// </summary>
    /// <returns>A collection of <see cref="AlbumStandoutSongItem" /> objects.</returns>
    private AlbumStandoutSongItem[]? GetOnlyStandoutSongs()
    {
        return (StandoutSongs is not null) switch
        {
            true => Array.FindAll(StandoutSongs, item => item.IsStandout),
            _ => null
        };
    }

    /// <summary>
    /// Formats the text to be used in an identifier string.
    /// </summary>
    /// <param name="input">The text to format.</param>
    /// <returns>A formatted string to use in an identifier string.</returns>
    private static string BuildIdentifierText(string input)
    {
        // Instantiate a StringBuilder object.
        StringBuilder stringBuilder = new();

        // Loop through each character in the input string.
        foreach (char character in input.ToCharArray())
        {
            // If the character is not a symbol or punctuation character,
            // then add it to the StringBuilder object.
            if (!char.IsSymbol(character) && !char.IsPunctuation(character))
            {
                // Logic for handling whether the character is a whitespace character.
                if (char.IsWhiteSpace(character))
                {
                    // If the character is a whitespace character,
                    // then append a dash character to the StringBuilder object.
                    stringBuilder.Append("-");
                }
                else
                {
                    // If the character is not a whitespace character,
                    // then append the character to the StringBuilder object.
                    stringBuilder.Append(character);
                }
            }
        }

        // Return the formatted string.
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Convert to the <see cref="AlbumData" /> type.
    /// </summary>
    /// <returns>The data as <see cref="AlbumData" />.</returns>
    public AlbumData ToAlbumData() => new()
    {
        Id = Id,
        PartitionKey = PartitionKey,
        SchemaVersion = SchemaVersion,
        Title = Title,
        Artist = Artist,
        StandoutSongs = StandoutSongs,
        AlbumArtUrl = AlbumArtUrl,
        AlbumUrl = AlbumUrl,
        IsBest = IsBest,
        Comments = Comments,
        ReleaseDate = ReleaseDate,
        ListYear = ListYear
    };
}