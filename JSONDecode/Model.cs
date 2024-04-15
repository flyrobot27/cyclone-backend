namespace CYCLONE.JSONDecode
{
    using System.Text.Json.Serialization;
    using CYCLONE.Types;

    public class Model
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public required MainType Type { get; set; }
		public required string ProcessName { get; set; }
		public required int LengthOfRun { get; set; }
		public required int NoOfCycles { get; set; }

		public required List<NetworkBlock> NetworkInput { get; set; }
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
		public required NetworkType Type { get; set; }
		public required int Label { get; set; }
		public required string Description { get; set; }
	}

	public class CombiBlock : NetworkBlock
	{
		public required DurationBlock Set { get; set; }
		public required List<ReferenceBlock> Followers { get; set; }
		public required List<ReferenceBlock> Preceders { get; set; }
	}

	public class NormalNetworkBlock : NetworkBlock
	{
		public required DurationBlock Set { get; set; }
		public required List<ReferenceBlock> Followers { get; set; }
	}

	public class QueueBlock: NetworkBlock
	{
		public required int NumberToBeGenerated { get; set; }
		public ResourceBlock? ResourceInput { get; set; }
	}

	public class FunctionConsolidateBlock: NetworkBlock
	{
		public required int NumberToConsolidate { get; set; }
		public required List<ReferenceBlock> Followers { get; set; }
	}

	public class FunctionCounterBlock: NetworkBlock
	{
		public required int Quantity { get; set; }
		public required List<ReferenceBlock> Followers { get; set; }
	}

	public class ReferenceBlock
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public required ReferenceType Type { get; set; }
		public required int Label { get; set; }
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
	[JsonDerivedType(typeof(NonStationaryBlock), nameof(DurationType.NST))]
	public class DurationBlock
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public required DurationType Type { get; set; }

		public required DistributionBlock Distribution { get; set; }
	}

	public class NonStationaryBlock : DurationBlock
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public required NSTCategory Category { get; set; }
		public required double Par1 { get; set; }
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
		public required DistributionType Type { get; set; }
	}

	public class DeterministicBlock: DistributionBlock
	{
		public required double Constant { get; set; }
	}

	public class ExponentialBlock: DistributionBlock
	{
		public required double Mean { get; set; }
	}

	public class UniformBlock : DistributionBlock
	{
		public required double Low { get; set; }
		public required double High { get; set; }
	}

	public class TriangularBlock : DistributionBlock
	{
		public required double Low { get; set; }
		public required double High { get; set; }
		public required double Mode { get; set; }
	}

	public class LognormalBlock : DistributionBlock
	{
		public required double Low { get; set; }
		public required double Scale { get; set; }
		public required double Shape { get; set; }
	}

	public class BetaBlock : DistributionBlock
	{
		public required double Low { get; set; }
		public required double High { get; set; }
		public required double Shape1 { get; set; }
		public required double Shape2 { get; set; }
	}

	public class NormalDistBlock : DistributionBlock
	{
		public required double Mean { get; set; }
		public required double Variance { get; set; }
	}

	public class ResourceBlock
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public required ResourceType Type { get; set; }
		public required int NoOfUnit { get; set; }
		public required string Description { get; set; }
		public List<CostBlock>? Cost { get; set; }
	}

	public class CostBlock
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public required ResourceCostType Type { get; set; }
		public required double Value { get; set; }
	}
}
