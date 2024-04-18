namespace CYCLONE.API.JSONDecode
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using CYCLONE.API.JSONDecode.Blocks;
    using CYCLONE.API.JSONDecode.Blocks.DistrbutionBlock;
    using CYCLONE.API.JSONDecode.Blocks.NetworkInput;
    using CYCLONE.API.JSONDecode.Converters;
    using CYCLONE.Template;
    using CYCLONE.Template.Interfaces;
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Types;
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
        private readonly Dictionary<string, CycloneElementBase> elements = [];
        private readonly Dictionary<string, IBlockHasFollowers> blockFollowers = [];
        private readonly Dictionary<string, IBlockHasPreceders> blockPreceders = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoder"/> class.
        /// </summary>
        /// <param name="JSONBody">The JSON to decode.</param>
        /// <exception cref="JsonException">If the Json is invalid.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "JSON is a term that is written this way")]
        public Decoder(string JSONBody)
        {
            // The Json Serializer Option will only be used here. It is safe to surpress the warning.
#pragma warning disable CA1869
            var deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                IncludeFields = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            };
#pragma warning restore CA1869
            deserializeOptions.Converters.Add(new NetworkBlockConverter());
            deserializeOptions.Converters.Add(new DurationBlockConverter());
            deserializeOptions.Converters.Add(new DistributionBlockConverter());
            deserializeOptions.Converters.Add(new NumberToStringConverter());
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
        /// <param name="engine">The <see cref="DiscreteEventEngine"/></param>
        /// <param name="debug">Whether to run in debug mode.</param>
        /// <returns>The Scenario object.</returns>
        public Scenario ToScenario(DiscreteEventEngine engine, bool debug = false)
        {
            var length = this.result.LengthOfRun;
            var processName = this.result.ProcessName;

            var scenario = new Scenario(processName: processName, engine: engine, length: length, debug: debug);

            this.ParseNetworkInput();
            this.SetBlockFollowersPreceders();

            scenario.InsertElements(this.elements.Values.ToArray());
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
                    ThrowHelper.ArgumentAssertion(bTri.Low < bTri.High, "Low cannot be greater than High");
                    bTri.Mode.ExceptionIfOutOfRange(bTri.Low, bTri.High, nameof(bTri.Mode), "Mode must be between Low and High");
                    dist = new Triangular(bTri.Low, bTri.High, bTri.Mode);
                    break;

                case DistributionType.UNIFORM:
                    var bUni = (UniformBlock)block;
                    ThrowHelper.ArgumentAssertion(bUni.Low < bUni.High, "Low cannot be greater than High");
                    dist = new Uniform(bUni.Low, bUni.High);
                    break;

                case DistributionType.LOGNORMAL:
                    var bLog = (LognormalBlock)block;
                    bLog.Shape.ExceptionIfNegative(nameof(bLog.Shape), "Shape cannot be negative");
                    dist = new LogNormal(bLog.Scale, bLog.Shape);
                    break;

                case DistributionType.BETA:
                    var bBet = (BetaBlock)block;
                    bBet.Shape1.ExceptionIfNegative(nameof(bBet.Shape1), "Shape1 cannot be negative");
                    bBet.Shape2.ExceptionIfNegative(nameof(bBet.Shape2), "Shape2 cannot be negative");
                    ThrowHelper.ArgumentAssertion(bBet.Low < bBet.High, "Low cannot be greater than High");
                    dist = new Beta(bBet.Shape1, bBet.Shape2, bBet.Low, bBet.High);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return dist;
        }

        private static CycloneElementBase InitializeBlock(NetworkBlock block)
        {
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
                    return new Queue(queueBlock.Label.ToString(), queueBlock.Description, initialLength: initialLength, multiplyByValue: Math.Max(1, queueBlock.NumberToBeGenerated));

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

        private void ParseNetworkInput()
        {
            foreach (NetworkBlock block in this.result.NetworkInput)
            {
                var initializedBlock = InitializeBlock(block);
                this.elements[block.Label] = initializedBlock;

                if (block is IBlockHasFollowers blockWithFollowers)
                {
                    this.blockFollowers[block.Label] = blockWithFollowers;
                }

                if (block is IBlockHasPreceders blockWithPreceders)
                {
                    this.blockPreceders[block.Label] = blockWithPreceders;
                }

                // Set counter limit
                if (initializedBlock is Counter counterBlock)
                {
                    counterBlock.Limit = this.result.NoOfCycles;
                }

                // Append to Combi list
                if (initializedBlock is Combi combiBlock)
                {
                    Queue.AddCombi(combiBlock);
                }
            }
        }

        private void SetBlockFollowersPreceders()
        {
            foreach (var entry in this.elements)
            {
                var block = entry.Value;
                var label = entry.Key;

                if (block is IAddFollowers<CycloneNetworkType> bFollowers)
                {
                    // Get the followers list
                    var followers = this.blockFollowers[label].Followers;
                    foreach (var follower in followers)
                    {
                        var actualBlock = this.GetActualBlockFromReference(follower);
                        bFollowers.AddFollowers(actualBlock);
                    }
                }

                if (block is IAddPreceders<CycloneNetworkType> bPreceders)
                {
                    // Get the preceders list
                    var preceders = this.blockPreceders[label].Preceders;
                    foreach (var preceder in preceders)
                    {
                        var actualBlock = this.GetActualBlockFromReference(preceder);
                        bPreceders.AddPreceders(actualBlock);
                    }
                }
            }
        }

        private CycloneElementBase GetActualBlockFromReference(ReferenceBlock reference)
        {
            var block = this.elements[reference.Value];
            var errorMessage = "Reference type does not match block type";

            // Check if type matches reference type
            switch (block.ElementType)
            {
                case CycloneNetworkType.COMBI:
                    if (reference.Type != ReferenceType.REF_COMBI)
                    {
                        throw new JsonException(errorMessage);
                    }

                    break;
                case CycloneNetworkType.QUEUE:
                    if (reference.Type != ReferenceType.REF_QUEUE)
                    {
                        throw new JsonException(errorMessage);
                    }

                    break;

                case CycloneNetworkType.NORMAL:
                    if (reference.Type != ReferenceType.REF_NORMAL)
                    {
                        throw new JsonException(errorMessage);
                    }

                    break;

                case CycloneNetworkType.FUNCTION_CONSOLIDATE:
                case CycloneNetworkType.FUNCTION_COUNTER:
                    if (reference.Type != ReferenceType.REF_FUNCTION)
                    {
                        throw new JsonException(errorMessage);
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }

            return block;
        }
    }
}
