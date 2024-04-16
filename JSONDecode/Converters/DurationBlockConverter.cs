namespace CYCLONE.JSONDecode.Converters
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using CYCLONE.JSONDecode.Blocks.DurationInput;
    using CYCLONE.JSONDecode.Extension;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Converts <see cref="DurationBlock"/> into appropriate subtypes.
    /// </summary>
    public class DurationBlockConverter : JsonConverter<DurationBlock>
    {
        /// <inheritdoc/>
        public override DurationBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var typestr = doc.RootElement.GetPropertyIgnoreCase("type").GetString()?.ToUpper();

            if (Enum.TryParse(typestr, out DurationType type))
            {
                var rawText = doc.RootElement.GetRawText();
                return type switch
                {
                    DurationType.STATIONARY => JsonSerializer.Deserialize<StationaryBlock>(rawText, options),
                    DurationType.NST => JsonSerializer.Deserialize<NonStationaryBlock>(rawText, options),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                throw new JsonException("Invalid Duration Type: " + Convert.ToString(typestr));
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DurationBlock value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}