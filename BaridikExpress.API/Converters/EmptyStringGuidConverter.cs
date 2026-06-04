using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaridikExpress.API.Converters;

public class EmptyStringGuidConverter : JsonConverter<Guid>
{
    public override Guid Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return Guid.Empty;

        if (reader.TokenType == JsonTokenType.Number)
            return Guid.Empty;

        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return Guid.Empty;

        if (Guid.TryParse(value, out var guid))
            return guid;

        return Guid.Empty;
    }

    public override void Write(
        Utf8JsonWriter writer,
        Guid value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}