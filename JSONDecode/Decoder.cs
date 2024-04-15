namespace CYCLONE.JSONDecode
{
    using System.Text.Json;
    using CYCLONE.Template;
    using CYCLONE.Types;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a JSON Decoder.
    /// </summary>
    public class Decoder
    {
        private readonly Model result;
        private readonly string jstring;

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoder"/> class.
        /// </summary>
        /// <param name="JSONBody">The JSON to decode.</param>
        /// <exception cref="JsonException">If the Json is invalid.</exception>
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

            this.jstring = JSONBody;
        }

        /// <summary>
        /// Returns the serialized JSON.
        /// </summary>
        /// <returns>The string of the serialized JSON.</returns>
        public string GetSerialized()
        {
            return this.jstring;
        }

        /// <summary>
        /// Converts the JSON to a <see cref="Scenario"/>.
        /// </summary>
        /// <returns>The Scenario object.</returns>
        public Scenario ToScenario()
        {
            var length = this.result.LengthOfRun;
            //// var terminationCount = this.result.NoOfCycles;
            var engine = new DiscreteEventEngine();

            var scenario = new Scenario(engine: engine, length: length);

            foreach (NetworkBlock block in this.result.NetworkInput)
            {
            }

            return scenario;
        }
    }
}
