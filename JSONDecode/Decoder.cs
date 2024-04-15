namespace CYCLONE.JSONDecode
{
	using CYCLONE.Template;
	using Simphony.Simulation;
	using System.Text.Json;

	public class Decoder
	{
		private readonly Model result;

		public Decoder(string JSONBody)
		{
			var deserializeOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				WriteIndented = true
			};
			var decoded = JsonSerializer.Deserialize<Model>(JSONBody, deserializeOptions);
			if (decoded != null)
			{
				this.result = decoded;
			}
			else
			{
				throw new JsonException("Invalid JSON");
			}
		}

		public string getSerialized()
		{
			return JsonSerializer.Serialize(this.result);
		}

		public Scenario ToScenario()
		{
			var length = result.LengthOfRun;
			var terminationCount = result.NoOfCycles;
			var engine = new DiscreteEventEngine();

			var scenario = new Scenario(engine, length, terminationCount);

			foreach (NetworkBlock block in result.NetworkInput)
			{
			}

			return scenario;
		}


	}
}
