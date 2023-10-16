using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using SmallsOnline.Web.Lib.Models.Database;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

namespace SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Songs;

/// <summary>
/// A form item for song data.
/// </summary>
public class SongDataFormItem : DatabaseItem, ISongData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SongDataFormItem" /> class.
    /// </summary>
    public SongDataFormItem()
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "favorites-of-tracks";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SongDataFormItem" /> class with the given list year.
    /// </summary>
    /// <param name="listYear">The list year for the item.</param>
    public SongDataFormItem(string? listYear)
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "favorites-of-tracks";
        ListYear = listYear;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SongDataFormItem" /> class from the given <see cref="ISongData" /> object.
    /// </summary>
    /// <param name="songData">The song data to import.</param>
    public SongDataFormItem(ISongData songData)
    {
        Id = songData.Id;
        PartitionKey = songData.PartitionKey;
        Title = songData.Title;
        Artist = songData.Artist;
        TrackArtUrl = songData.TrackArtUrl;
        TrackUrl = songData.TrackUrl;
        ReleaseDate = songData.ReleaseDate;
        Comments = songData.Comments;
        ListYear = songData.ListYear;
    }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("trackTitle")]
    public string? Title { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("trackArtist")]
    public string? Artist { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("trackArtUrl")]
    public string? TrackArtUrl { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("trackUrl")]
    public string? TrackUrl { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("trackReleaseDate")]
    public DateTimeOffset? ReleaseDate { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackComments")]
    public string? Comments { get; set; }

    /// <inheritdoc />
    [Required]
    [JsonPropertyName("listYear")]
    public string? ListYear { get; set; }

    /// <inheritdoc />
    /// <remarks>Does not return when converted to JSON.</remarks>
    [JsonIgnore]
    public string? SongId
    {
        get
        {
            // Instantiate a StringBuilder object.
            StringBuilder stringBuilder = new();

            // If the Artist property is not null or empty,
            // format the text and append it to the string builder.
            if (Artist is not null)
            {
                stringBuilder.Append(BuildIdentifierText(Artist));
            }

            // If the Title property is not null or empty,
            // format the text and append it to the string builder.
            if (Title is not null)
            {
                stringBuilder.Append("_");
                stringBuilder.Append(BuildIdentifierText(Title));
            }

            // Return the string builder's text and ensure it is lowercase.
            return stringBuilder.ToString().ToLower();
        }
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
    /// Convert to the <see cref="SongData" /> type.
    /// </summary>
    /// <returns>The data as <see cref="SongData" />.</returns>
    public SongData ToSongData() => new()
    {
        Id = Id,
        PartitionKey = PartitionKey,
        Title = Title,
        Artist = Artist,
        TrackArtUrl = TrackArtUrl,
        TrackUrl = TrackUrl,
        ReleaseDate = ReleaseDate,
        Comments = Comments,
        ListYear = ListYear
    };
}