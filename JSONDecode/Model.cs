namespace CYCLONE.JSONDecode
{
    using System.Text.Json.Serialization;
    using CYCLONE.Types;

    public class Model
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public MainType Type { get; set; }

        required public string ProcessName { get; set; }

        required public int LengthOfRun { get; set; }

        required public int NoOfCycles { get; set; }

        required public List<NetworkBlock> NetworkInput { get; set; }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
    [JsonDerivedType(typeof(CombiBlock), nameof(NetworkType.COMBI))]
    [JsonDerivedType(typeof(NormalNetworkBlock), nameof(NetworkType.NORMAL))]
    [JsonDerivedType(typeof(QueueBlock), nameof(NetworkType.QUEUE))]
    [JsonDerivedType(typeof(FunctionConsolidateBlock), nameof(NetworkType.FUNCTION_CONSOLIDATE))]
    [JsonDerivedType(typeof(FunctionCounterBlock), nameof(NetworkType.FUNCTION_COUNTER))]
    public class NetworkBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public NetworkType Type { get; set; }

        required public int Label { get; set; }

        required public string Description { get; set; }
    }

    public class CombiBlock : NetworkBlock
    {
        required public DurationBlock Set { get; set; }

        required public List<ReferenceBlock> Followers { get; set; }

        required public List<ReferenceBlock> Preceders { get; set; }
    }

    public class NormalNetworkBlock : NetworkBlock
    {
        required public DurationBlock Set { get; set; }

        required public List<ReferenceBlock> Followers { get; set; }
    }

    public class QueueBlock : NetworkBlock
    {
        required public int NumberToBeGenerated { get; set; }

        public ResourceBlock? ResourceInput { get; set; }
    }

    public class FunctionConsolidateBlock : NetworkBlock
    {
        required public int NumberToConsolidate { get; set; }

        required public List<ReferenceBlock> Followers { get; set; }
    }

    public class FunctionCounterBlock : NetworkBlock
    {
        required public int Quantity { get; set; }

        required public List<ReferenceBlock> Followers { get; set; }
    }

    public class ReferenceBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ReferenceType Type { get; set; }

        required public int Label { get; set; }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
    [JsonDerivedType(typeof(NonStationaryBlock), nameof(DurationType.NST))]
    public class DurationBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public DurationType Type { get; set; }

        required public DistributionBlock Distribution { get; set; }
    }

    public class NonStationaryBlock : DurationBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public NSTCategory Category { get; set; }

        required public double Par1 { get; set; }

        public double? Par2 { get; set; }

        public int? Seed { get; set; }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
    [JsonDerivedType(typeof(DeterministicBlock), nameof(DistributionType.DETERMINISTIC))]
    [JsonDerivedType(typeof(ExponentialBlock), nameof(DistributionType.EXPONENTIAL))]
    [JsonDerivedType(typeof(UniformBlock), nameof(DistributionType.UNIFORM))]
    [JsonDerivedType(typeof(TriangularBlock), nameof(DistributionType.TRIANGULAR))]
    [JsonDerivedType(typeof(LognormalBlock), nameof(DistributionType.LOGNORMAL))]
    [JsonDerivedType(typeof(BetaBlock), nameof(DistributionType.BETA))]
    [JsonDerivedType(typeof(NormalDistBlock), nameof(DistributionType.NORMAL))]
    public class DistributionBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public DistributionType Type { get; set; }
    }

    public class DeterministicBlock : DistributionBlock
    {
        required public double Constant { get; set; }
    }

    public class ExponentialBlock : DistributionBlock
    {
        required public double Mean { get; set; }
    }

    public class UniformBlock : DistributionBlock
    {
        required public double Low { get; set; }

        required public double High { get; set; }
    }

    public class TriangularBlock : DistributionBlock
    {
        required public double Low { get; set; }

        required public double High { get; set; }

        required public double Mode { get; set; }
    }

    public class LognormalBlock : DistributionBlock
    {
        required public double Low { get; set; }

        required public double Scale { get; set; }

        required public double Shape { get; set; }
    }

    public class BetaBlock : DistributionBlock
    {
        required public double Low { get; set; }

        required public double High { get; set; }

        required public double Shape1 { get; set; }

        required public double Shape2 { get; set; }
    }

    public class NormalDistBlock : DistributionBlock
    {
        required public double Mean { get; set; }

        required public double Variance { get; set; }
    }

    public class ResourceBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ResourceType Type { get; set; }

        required public int NoOfUnit { get; set; }

        required public string Description { get; set; }

        public List<CostBlock>? Cost { get; set; }
    }

    public class CostBlock
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ResourceCostType Type { get; set; }

        required public double Value { get; set; }
    }
}
