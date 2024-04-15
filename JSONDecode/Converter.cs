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
					NetworkType.NORMAL => JsonSerializer.Deserialize<NormalNetworkBlock>(doc.RootElement.GetRawText()),
					NetworkType.QUEUE => JsonSerializer.Deserialize<QueueBlock>(doc.RootElement.GetRawText()),
					NetworkType.FUNCTION_CONSOLIDATE => JsonSerializer.Deserialize<FunctionConsolidateBlock>(doc.RootElement.GetRawText()),
					NetworkType.FUNCTION_COUNTER => JsonSerializer.Deserialize<FunctionCounterBlock>(doc.RootElement.GetRawText()),
					_ => throw new NotImplementedException(),
				};
			}
			else
			{
				throw new JsonException("Invalid Network Type: " + Convert.ToString(typestr));
			}
		}

		public override void Write(Utf8JsonWriter writer, NetworkBlock value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}

	public class DurationBlockConverter : JsonConverter<DurationBlock>
	{
		public override DurationBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using var doc = JsonDocument.ParseValue(ref reader);
			var typestr = doc.RootElement.GetProperty(@"Type").GetString();

			if (Enum.TryParse(typestr, out DurationType type))
			{
				if (type == DurationType.STATIONARY)
				{
					return JsonSerializer.Deserialize<DurationBlock>(doc.RootElement.GetRawText());
				}
				else if (type == DurationType.NST)
				{
					return JsonSerializer.Deserialize<NonStationaryBlock>(doc.RootElement.GetRawText());
				}

				throw new NotImplementedException();
			}
			else
			{
				throw new JsonException("Invalid Duration Type: " + Convert.ToString(typestr));
			}
		}

		public override void Write(Utf8JsonWriter writer, DurationBlock value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}

	public class DistributionBlockConverter : JsonConverter<DistributionBlock>
	{
		public override DistributionBlock? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using var doc = JsonDocument.ParseValue(ref reader);
			var typestr = doc.RootElement.GetProperty(@"Type").GetString();

			if (Enum.TryParse(typestr, out DistributionType type))
			{
				return type switch
				{
					DistributionType.DETERMINISTIC => JsonSerializer.Deserialize<DeterministicBlock>(doc.RootElement.GetRawText()),
					DistributionType.NORMAL => JsonSerializer.Deserialize<NormalDistBlock>(doc.RootElement.GetRawText()),
					DistributionType.TRIANGULAR => JsonSerializer.Deserialize<TriangularBlock>(doc.RootElement.GetRawText()),
					DistributionType.LOGNORMAL => JsonSerializer.Deserialize<LognormalBlock>(doc.RootElement.GetRawText()),
					DistributionType.BETA => JsonSerializer.Deserialize<BetaBlock>(doc.RootElement.GetRawText()),
					DistributionType.EXPONENTIAL => JsonSerializer.Deserialize<ExponentialBlock>(doc.RootElement.GetRawText()),
					DistributionType.UNIFORM => JsonSerializer.Deserialize<UniformBlock>(doc.RootElement.GetRawText()),
					_ => throw new NotImplementedException(),
				};
			}
			else
			{
				throw new JsonException("Invalid Distribution Type: " + Convert.ToString(typestr));
			}
		}

		public override void Write(Utf8JsonWriter writer, DistributionBlock value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}

