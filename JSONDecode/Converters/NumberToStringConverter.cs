namespace CYCLONE.API.JSONDecode.Converters
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Converts JSON number to a string for all situations.
    /// </summary>
    public class NumberToStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString();
            }
            return reader.GetString() ?? string.Empty;
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
