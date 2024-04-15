namespace CYCLONE.JSONDecode
{
    using System.Text.Json;
    using CYCLONE.Template;
    using Simphony.Simulation;

    public class Decoder
    {
        private readonly Model result;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "JSON is a term that is written this way")]
        public Decoder(string JSONBody)
        {
            var deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
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
            return JsonSerializer.Serialize((object)this.result);
        }

        public Scenario ToScenario()
        {
            var length = this.result.LengthOfRun;
            var terminationCount = this.result.NoOfCycles;
            var engine = new DiscreteEventEngine();

            var scenario = new Scenario(engine, length, terminationCount);

            foreach (NetworkBlock block in this.result.NetworkInput)
            {
            }

            return scenario;
        }


    }
}
