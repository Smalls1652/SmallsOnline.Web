namespace SmallsOnline.Web.Lib.Models.Json;

/// <summary>
/// Converter for the <see cref="DateTimeOffset"/> type during serialization and deserialization of JSON data.
/// </summary>
public class JsonDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Convert the input string to a DateTimeOffset.
        DateTimeOffset inputTime = DateTimeOffset.Parse($"{reader.GetString()}");

        // Return the UTC time.
        return inputTime.UtcDateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        // Write the UTC time.
        writer.WriteStringValue(value.UtcDateTime.ToString("s"));
    }
}