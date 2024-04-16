namespace CYCLONE.JSONDecode.Converters
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using CYCLONE.JSONDecode.Blocks.NetworkInput;
    using CYCLONE.JSONDecode.Extension;
    using CYCLONE.Types;

    /// <summary>
    /// Converts <see cref="NetworkBlock"/> into appropriate subtypes.
    /// </summary>
    public class NetworkBlockConverter : JsonConverter<NetworkBlock>
    {
        /// <inheritdoc/>
        public override NetworkBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var typestr = doc.RootElement.GetPropertyIgnoreCase("Type").GetString()?.ToUpper();

            if (Enum.TryParse(typestr, out CycloneNetworkType type))
            {
                var rawText = doc.RootElement.GetRawText();
                return type switch
                {
                    CycloneNetworkType.COMBI => JsonSerializer.Deserialize<CombiBlock>(rawText, options),
                    CycloneNetworkType.NORMAL => JsonSerializer.Deserialize<NormalNetworkBlock>(rawText, options),
                    CycloneNetworkType.QUEUE => JsonSerializer.Deserialize<QueueBlock>(rawText, options),
                    CycloneNetworkType.FUNCTION_CONSOLIDATE => JsonSerializer.Deserialize<FunctionConsolidateBlock>(rawText, options),
                    CycloneNetworkType.FUNCTION_COUNTER => JsonSerializer.Deserialize<FunctionCounterBlock>(rawText, options),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                throw new JsonException("Invalid Network Type: " + Convert.ToString(typestr));
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, NetworkBlock value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}