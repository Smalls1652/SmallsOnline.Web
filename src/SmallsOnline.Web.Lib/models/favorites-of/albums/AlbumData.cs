using System.Text;
using SmallsOnline.Web.Lib.Models.Database;

namespace SmallsOnline.Web.Lib.Models.FavoritesOf.Albums;

public class AlbumData : DatabaseItem, IAlbumData
{
    public AlbumData() { }

    [JsonPropertyName("@schemaVersion")]
    public string? SchemaVersion { get; set; }

    [JsonPropertyName("albumTitle")]
    public string? Title { get; set; }

    [JsonPropertyName("albumArtist")]
    public string? Artist { get; set; }

    [JsonPropertyName("albumStandoutSongs")]
    public IEnumerable<AlbumStandoutSongItem>? StandoutSongs { get; set; }

    [JsonIgnore]
    public IEnumerable<AlbumStandoutSongItem>? OnlyStandoutSongs => GetOnlyStandoutSongs();

    [JsonPropertyName("albumStandoutTracks")]
    public IEnumerable<AlbumStandoutSong>? StandoutTracks { get; set; }

    [JsonPropertyName("albumArtUrl")]
    public string? AlbumArtUrl { get; set; }

    [JsonPropertyName("albumUrl")]
    public string? AlbumUrl { get; set; }

    [JsonPropertyName("albumIsBest")]
    public bool IsBest { get; set; }

    [JsonPropertyName("albumComments")]
    public string? Comments { get; set; }

    [JsonPropertyName("albumReleaseDate")]
    public DateTimeOffset? ReleaseDate { get; set; }

    [JsonPropertyName("listYear")]
    public string? ListYear { get; set; }

    [JsonIgnore]
    public string? AlbumId
    {
        get
        {
            StringBuilder stringBuilder = new();

            if (Artist is not null)
            {
                stringBuilder.Append(BuildIdentifierText(Artist));
            }

            if (Title is not null)
            {
                stringBuilder.Append("_");
                stringBuilder.Append(BuildIdentifierText(Title));
            }

            return stringBuilder.ToString().ToLower();
        }
    }

    private List<AlbumStandoutSongItem>? GetOnlyStandoutSongs()
    {
        return (StandoutSongs is not null) switch
        {
            true => StandoutSongs.ToList().FindAll(item => item.IsStandout),
            _ => null
        };
    }

    private static string BuildIdentifierText(string input)
    {
        StringBuilder stringBuilder = new();

        foreach (char character in input.ToCharArray())
        {
            if (!char.IsSymbol(character) && !char.IsPunctuation(character))
            {
                if (char.IsWhiteSpace(character))
                {
                    stringBuilder.Append("-");
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }
        }

        return stringBuilder.ToString();
    }
}