namespace SmallsOnline.Web.Lib.Models.Json;

public class JsonDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        DateTimeOffset inputTime = DateTimeOffset.Parse($"{reader.GetString()}");
        return inputTime.UtcDateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.UtcDateTime.ToString("s"));
    }
}