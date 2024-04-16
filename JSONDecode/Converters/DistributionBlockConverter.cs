namespace CYCLONE.JSONDecode.Converters
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using CYCLONE.JSONDecode.Blocks.DistrbutionBlock;
    using CYCLONE.JSONDecode.Extension;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Converts <see cref="DistributionBlock"/> into appropriate subtypes.
    /// </summary>
    public class DistributionBlockConverter : JsonConverter<DistributionBlock>
    {
        /// <inheritdoc/>
        public override DistributionBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var typestr = doc.RootElement.GetPropertyIgnoreCase("Type").GetString()?.ToUpper();

            if (Enum.TryParse(typestr, out DistributionType type))
            {
                var rawText = doc.RootElement.GetRawText();
                return type switch
                {
                    DistributionType.DETERMINISTIC => JsonSerializer.Deserialize<DeterministicBlock>(rawText, options),
                    DistributionType.NORMAL => JsonSerializer.Deserialize<NormalDistBlock>(rawText, options),
                    DistributionType.TRIANGULAR => JsonSerializer.Deserialize<TriangularBlock>(rawText, options),
                    DistributionType.LOGNORMAL => JsonSerializer.Deserialize<LognormalBlock>(rawText, options),
                    DistributionType.BETA => JsonSerializer.Deserialize<BetaBlock>(rawText, options),
                    DistributionType.EXPONENTIAL => JsonSerializer.Deserialize<ExponentialBlock>(rawText, options),
                    DistributionType.UNIFORM => JsonSerializer.Deserialize<UniformBlock>(rawText, options),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                throw new JsonException("Invalid Distribution Type: " + Convert.ToString(typestr));
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DistributionBlock value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}