using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using SmallsOnline.Web.Lib.Models.Database;
using SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

namespace SmallsOnline.Web.AdminSite.Server.Models.FavoritesOf.Albums;

public class AlbumDataFormItem : DatabaseItem, IAlbumData
{
    public AlbumDataFormItem()
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "favorites-of-albums";
        SchemaVersion = "2.0";
    }

    public AlbumDataFormItem(string? listYear)
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = "favorites-of-albums";
        ListYear = listYear;
        SchemaVersion = "2.0";
    }

    public AlbumDataFormItem(IAlbumData albumData)
    {
        Id = albumData.Id;
        PartitionKey = albumData.PartitionKey;

        SchemaVersion = albumData.SchemaVersion is null && albumData.StandoutTracks is null || albumData.StandoutSongs is not null ? "2.0" : "1.0";

        Title = albumData.Title;
        Artist = albumData.Artist;
        StandoutSongs = albumData.StandoutSongs;

        if (albumData.StandoutTracks is not null)
        {
            _standoutTracks = new(albumData.StandoutTracks);
        }

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
    [JsonPropertyName("albumStandoutTracks")]
    public IEnumerable<AlbumStandoutSong>? StandoutTracks
    {
        get => _standoutTracks;
        set => _standoutTracks = value is not null ? new(value) : null;
    }

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

    private List<AlbumStandoutSong>? _standoutTracks;
    private List<AlbumStandoutSongItem>? _standoutSongs;

    public void AddStandoutSong() => AddStandoutSong(new());
    public void AddStandoutSong(AlbumStandoutSong standoutSong)
    {
        if (_standoutTracks is null)
        {
            _standoutTracks = new();
        }

        _standoutTracks.Add(standoutSong);
    }

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

    public AlbumData ToAlbumData() => new()
    {
        Id = Id,
        PartitionKey = PartitionKey,
        SchemaVersion = SchemaVersion,
        Title = Title,
        Artist = Artist,
        StandoutSongs = StandoutSongs,
        StandoutTracks = StandoutTracks,
        AlbumArtUrl = AlbumArtUrl,
        AlbumUrl = AlbumUrl,
        IsBest = IsBest,
        Comments = Comments,
        ReleaseDate = ReleaseDate,
        ListYear = ListYear
    };
}