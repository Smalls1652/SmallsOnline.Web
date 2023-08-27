using System.Text;
using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Songs;

/// <summary>
/// Contains data for a favorite song.
/// </summary>
public class SongData : DatabaseItem, ISongData
{
    public SongData() {}

    /// <inheritdoc />
    [JsonPropertyName("trackTitle")]
    public string? Title { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackArtist")]
    public string? Artist { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackArtUrl")]
    public string? TrackArtUrl { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackUrl")]
    public string? TrackUrl { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackReleaseDate")]
    public DateTimeOffset? ReleaseDate { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("trackComments")]
    public string? Comments { get; set; }

    /// <inheritdoc />
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
}