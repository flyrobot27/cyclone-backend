namespace CYCLONE.JSONDecode
{
	using System.Text.Json.Serialization;
	using System.Text.Json;

	public class NetworkBlockConverter : JsonConverter<NetworkBlock>
	{
		public override NetworkBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using var doc = JsonDocument.ParseValue(ref reader);
			var typestr = doc.RootElement.GetProperty(@"Type").GetString();

			if (Enum.TryParse(typestr, out NetworkType type))
			{
				return type switch
				{
					NetworkType.COMBI => JsonSerializer.Deserialize<CombiBlock>(doc.RootElement.GetRawText()),
					NetworkType.NORMAL => JsonSerializer.Deserialize<NormalBlock>(doc.RootElement.GetRawText()),
					NetworkType.QUEUE => JsonSerializer.Deserialize<QueueBlock>(doc.RootElement.GetRawText()),
					NetworkType.FUNCTION_CONSOLIDATE => JsonSerializer.Deserialize<FunctionConsolidateBlock>(doc.RootElement.GetRawText()),
					NetworkType.FUNCTION_COUNTER => JsonSerializer.Deserialize<FunctionCounterBlock>(doc.RootElement.GetRawText()),
					_ => throw new NotImplementedException(),
				};
			}
			else
			{
				throw new JsonException("Invalid Type: " + Convert.ToString(typestr));
			}
		}

		public override void Write(Utf8JsonWriter writer, NetworkBlock value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}

	
}
