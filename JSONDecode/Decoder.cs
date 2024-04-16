namespace CYCLONE.JSONDecode
{
    using System.Text.Json;

    using CYCLONE.JSONDecode.Converters;
    using CYCLONE.Template;
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;

    using Simphony;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a JSON Decoder.
    /// </summary>
    public class Decoder
    {
        private readonly ModelBlock result;
        private readonly string jstring;
        private readonly List<CycloneElementBase> elements = [];

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
                IncludeFields = true,
            };
            deserializeOptions.Converters.Add(new NetworkBlockConverter());
            deserializeOptions.Converters.Add(new DurationBlockConverter());
            deserializeOptions.Converters.Add(new DistributionBlockConverter());
            var decoded = JsonSerializer.Deserialize<ModelBlock>(JSONBody, deserializeOptions);
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
                var initializedBlock = InitializeBlock(block);
                this.elements.Add(initializedBlock);
            }

            return scenario;
        }

        private static Distribution GetDistribution(DistributionBlock block)
        {
            var type = block.Type;
            Distribution dist;
            switch (type)
            {
                case DistributionType.DETERMINISTIC:
                    var bDet = (DeterministicBlock)block;
                    dist = new Constant(bDet.Constant);
                    break;
                
                case DistributionType.EXPONENTIAL:
                    var bExp = (ExponentialBlock)block;
                    bExp.Mean.ExceptionIfNegative(nameof(bExp.Mean), "Mean cannot be negative");
                    dist = new Exponential(bExp.Mean);
                    break;
                
                case DistributionType.NORMAL:
                    var bNor = (NormalDistBlock)block;
                    bNor.Variance.ExceptionIfNegative(nameof(bNor.Variance), "Variance cannot be negative");
                    dist = new Simphony.Mathematics.Normal(bNor.Mean, Math.Sqrt(bNor.Variance));
                    break;
                
                case DistributionType.TRIANGULAR:
                    var bTri = (TriangularBlock)block;

                    if (bTri.Low > bTri.High)
                    {
                        throw new ArgumentException("Low cannot be greater than High");
                    }

                    bTri.Mode.ExceptionIfOutOfRange(bTri.Low, bTri.High, nameof(bTri.Mode), "Mode must be between Low and High");

                    // TODO: Implement Triangular distribution
                    throw new NotImplementedException();
                
                case DistributionType.UNIFORM:
                    var bUni = (UniformBlock)block;

                    if (bUni.Low > bUni.High)
                    {
                        throw new ArgumentException("Low cannot be greater than High");
                    }

                    // TODO: Implement Uniform distribution
                    throw new NotImplementedException();
                
                case DistributionType.LOGNORMAL:
                    var bLog = (LognormalBlock)block;
                    bLog.Shape.ExceptionIfNegative(nameof(bLog.Shape), "Shape cannot be negative");

                    // TODO: Implement Lognormal distribution
                    throw new NotImplementedException();
                
                case DistributionType.BETA:
                    var bBet = (BetaBlock)block;
                    bBet.Shape1.ExceptionIfNegative(nameof(bBet.Shape1), "Shape1 cannot be negative");
                    bBet.Shape2.ExceptionIfNegative(nameof(bBet.Shape2), "Shape2 cannot be negative");

                    if (bBet.Low > bBet.High)
                    {
                        throw new ArgumentException("Low cannot be greater than High");
                    }

                    //TODO: Implement Beta distribution
                    throw new NotImplementedException();
                
                default:
                    throw new NotImplementedException();
            }

            return dist;
        }

        private static CycloneElementBase InitializeBlock(NetworkBlock block)
        {
            Console.WriteLine(block.GetType());
            switch (block.Type)
            {
                case CycloneNetworkType.COMBI:
                    CombiBlock combiBlock = (CombiBlock)block;
                    Distribution set = GetDistribution(combiBlock.Set.Distribution);
                    return new Combi(combiBlock.Label.ToString(), combiBlock.Description, set);
                
                case CycloneNetworkType.QUEUE:
                    QueueBlock queueBlock = (QueueBlock)block;
                    queueBlock.NumberToBeGenerated.ExceptionIfNegative(nameof(queueBlock.NumberToBeGenerated), "Number to be generated cannot be negative");
                    int initialLength = GetQueueInitialLength(queueBlock);
                    return new Queue(queueBlock.Label.ToString(), queueBlock.Description, initialLength: initialLength, multiplyByValue: queueBlock.NumberToBeGenerated);
                
                case CycloneNetworkType.NORMAL:
                    NormalNetworkBlock normalNetworkBlock = (NormalNetworkBlock)block;
                    Distribution duration = GetDistribution(normalNetworkBlock.Set.Distribution);
                    return new Template.Normal(normalNetworkBlock.Label.ToString(), normalNetworkBlock.Description, duration);

                case CycloneNetworkType.FUNCTION_CONSOLIDATE:
                    FunctionConsolidateBlock functionConsolidateBlock = (FunctionConsolidateBlock)block;
                    functionConsolidateBlock.NumberToConsolidate.ExceptionIfNegative(nameof(functionConsolidateBlock.NumberToConsolidate), "Number to consolidate cannot be negative");
                    return new Consolidate(functionConsolidateBlock.Label.ToString(), functionConsolidateBlock.Description, divideByValue: functionConsolidateBlock.NumberToConsolidate);
                
                case CycloneNetworkType.FUNCTION_COUNTER:
                    FunctionCounterBlock functionCounterBlock = (FunctionCounterBlock)block;
                    return new Counter(functionCounterBlock.Label.ToString(), functionCounterBlock.Description);
                
                default:
                    throw new NotImplementedException();
            }
        }

        private static int GetQueueInitialLength(QueueBlock queueBlock)
        {
            var initialLength = 0;
            if (queueBlock.ResourceInput != null)
            {
                initialLength = queueBlock.ResourceInput.NoOfUnit;

                // TEMP: Cost is not implemented
                if (queueBlock.ResourceInput.Cost?.Count > 0)
                {
                    throw new NotImplementedException();
                }
            }

            return initialLength;
        }
    }
}
