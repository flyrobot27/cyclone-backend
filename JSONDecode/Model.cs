namespace CYCLONE.JSONDecode
{
	using System.Text.Json.Serialization;

	public class Model
	{
		public required MainType Type { get; set; }
		public required string ProcessName { get; set; }
		public required int LengthOfRun { get; set; }
		public required int NoOfCycle { get; set; }

		[JsonConverter(typeof(NetworkBlockConverter))]
		public required NetworkBlock[] NetworkInput { get; set; }
	}

	public class NetworkBlock
	{
		public required NetworkType Type { get; set; }
		public required int Label { get; set; }
		public required string Description { get; set; }
	}

	public class CombiBlock : NetworkBlock
	{
		[JsonConverter(typeof(DurationBlockConverter))]
		public required DurationBlock Set { get; set; }
		public required ReferenceBlock[] Followers { get; set; }
		public required ReferenceBlock[] Preceders { get; set; }
	}

	public class NormalNetworkBlock : NetworkBlock
	{
		[JsonConverter(typeof(DurationBlockConverter))]
		public required DurationBlock Set { get; set; }
		public required ReferenceBlock[] Followers { get; set; }
	}

	public class QueueBlock: NetworkBlock
	{
		public required int NumberToBeGenerated { get; set; }
		public ResourceBlock? ResourceInput { get; set; }
	}

	public class FunctionConsolidateBlock: NetworkBlock
	{
		public required int NumberToConsolidate { get; set; }
		public required ReferenceBlock[] Followers { get; set; }
	}

	public class FunctionCounterBlock: NetworkBlock
	{
		public required int Quantity { get; set; }
		public required ReferenceBlock[] Followers { get; set; }
	}

	public class ReferenceBlock
	{
		public required ReferenceType Type { get; set; }
		public required int Label { get; set; }
	}

	public class DurationBlock
	{
		public required DurationType Type { get; set; }

		[JsonConverter(typeof(DistributionBlockConverter))]
		public required DistributionBlock Distribution { get; set; }
	}

	public class NonStationaryBlock : DurationBlock
	{
		public required NSTCategory Category { get; set; }
		public required double Par1 { get; set; }
		public double? Par2 { get; set; }
		public int? Seed { get; set; }
	}

	public class DistributionBlock
	{
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
		public required ResourceType Type { get; set; }
		public required int NoOfUnit { get; set; }
		public required string Description { get; set; }
		public CostBlock[]? Cost { get; set; }
	}

	public class CostBlock
	{
		public required ResourceCostType Type { get; set; }
		public required double Value { get; set; }
	}
}
