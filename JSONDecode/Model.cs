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
		public required DurationBlock Set { get; set; }
		public required ReferenceBlock[] Followers { get; set; }
		public required ReferenceBlock[] Preceders { get; set; }
	}

	public class NormalBlock : NetworkBlock
	{
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

	// TODO: Implement blocks for duration, resource and distribution
	public class DurationBlock
	{
		public required CYCLONE.DurationType Type { get; set; }
		public required DistributionBlock Distribution { get; set; }
	}

	public class DistributionBlock
	{
		public required CYCLONE.DistributionType Type { get; set; }
	}

	public class ResourceBlock
	{

	}
}
