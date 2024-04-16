namespace CYCLONE.JSONDecode
{
    using System.Text.Json.Serialization;
    using CYCLONE.Types;

    /// <summary>
    /// Model Block class representing the JSON structure.
    /// </summary>
    public class ModelBlock
    {
        /// <summary>
        /// Gets or sets the type of the main block.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public MainType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        required public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the length of the run.
        /// </summary>
        required public int LengthOfRun { get; set; }

        /// <summary>
        /// Gets or sets the number of cycles.
        /// </summary>
        required public int NoOfCycles { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="NetworkBlock"/> in the Network Input.
        /// </summary>
        required public List<NetworkBlock> NetworkInput { get; set; }
    }

    /// <summary>
    /// Network block class representing the JSON structure Network Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class NetworkBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="NetworkBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public CycloneNetworkType Type { get; set; }

        /// <summary>
        /// Gets or sets the label of the <see cref="NetworkBlock"/>.
        /// </summary>
        required public string Label { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="NetworkBlock"/>.
        /// </summary>
        required public string Description { get; set; }
    }

    /// <summary>
    /// Combi block class representing the JSON structure in Network Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class CombiBlock : NetworkBlock, IBlockHasFollowers, IBlockHasPreceders
    {
        /// <summary>
        /// Gets or sets the <see cref="DurationBlock"/> of the <see cref="CombiBlock"/>.
        /// </summary>
        required public DurationBlock Set { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> follwing the <see cref="CombiBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> preceding the <see cref="CombiBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Preceders { get; set; }
    }

    /// <summary>
    /// Normal network block class representing the JSON structure in Network Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class NormalNetworkBlock : NetworkBlock, IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the <see cref="DurationBlock"/> of the <see cref="NormalNetworkBlock"/>.
        /// </summary>
        required public DurationBlock Set { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> follwing the <see cref="NormalNetworkBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }
    }

    /// <summary>
    /// Queue block class representing the JSON structure in Network Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class QueueBlock : NetworkBlock
    {
        /// <summary>
        /// Gets or sets the Number of entities to be generated.
        /// </summary>
        required public int NumberToBeGenerated { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ResourceBlock"/> of the <see cref="QueueBlock"/>.
        /// </summary>
        public ResourceBlock? ResourceInput { get; set; }
    }

    /// <summary>
    /// Function Consolidate block class representing the JSON structure in Network Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class FunctionConsolidateBlock : NetworkBlock, IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the Number of entities to consolidate.
        /// </summary>
        required public int NumberToConsolidate { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> following the <see cref="FunctionConsolidateBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }
    }

    /// <summary>
    /// Function Counter block class representing the JSON structure in Network Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class FunctionCounterBlock : NetworkBlock, IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the quantity of elements initially in the counter.
        /// </summary>
        required public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> following the <see cref="FunctionCounterBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }
    }

    /// <summary>
    /// Reference block class representing the JSON structure that is used to reference another <see cref="NetworkBlock"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class ReferenceBlock
    {
        /// <summary>
        /// Gets or sets the type of the reference block.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ReferenceType Type { get; set; }

        /// <summary>
        /// Gets or sets the label of the <see cref="NetworkBlock"/> the reference block is referring to.
        /// </summary>
        required public string Value { get; set; }
    }

    /// <summary>
    /// Duration block class representing the JSON structure for Duration Input.
    /// </summary>
    //[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
    //[JsonDerivedType(typeof(NonStationaryBlock), nameof(DurationType.NST))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class DurationBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="DurationBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public DurationType Type { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DistributionBlock"/> of the <see cref="DurationBlock"/>.
        /// </summary>
        required public DistributionBlock Distribution { get; set; }
    }

    /// <summary>
    /// Stationary block class representing the JSON structure for Duration Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class StationaryBlock : DurationBlock
    {
        // Empty
    }

    /// <summary>
    /// Non Stationary block class representing the JSON structure for Duration Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class NonStationaryBlock : DurationBlock
    {
        /// <summary>
        /// Gets or Sets the Non Staionary Category of the <see cref="NonStationaryBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public NSTCategory Category { get; set; }

        /// <summary>
        /// Gets or Sets the Par1 of the <see cref="NonStationaryBlock"/>.
        /// </summary>
        required public double Par1 { get; set; }

        /// <summary>
        /// Gets or Sets the Par2 of the <see cref="NonStationaryBlock"/>. Only used if <see cref="Category"/> is <see cref="NSTCategory.SECOND"/>.
        /// </summary>
        public double? Par2 { get; set; }

        /// <summary>
        /// Gets or Sets the Seed of the <see cref="NonStationaryBlock"/>. Only used if <see cref="Category"/> is <see cref="NSTCategory.SECOND"/>.
        /// </summary>
        public int? Seed { get; set; }
    }

    /// <summary>
    /// Distribution block class representing the JSON structure for a Statistic Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class DistributionBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="DistributionBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public DistributionType Type { get; set; }
    }

    /// <summary>
    /// Deterministic Block class represents the JSON structure of a Deterministic Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class DeterministicBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the constant value of the <see cref="Simphony.Mathematics.Constant"/> Distribution.
        /// </summary>
        required public double Constant { get; set; }
    }

    /// <summary>
    /// Exponential Block class represents the JSON structure of an Exponential Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class ExponentialBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the mean value of the <see cref="Simphony.Mathematics.Exponential"/> Distribution.
        /// </summary>
        required public double Mean { get; set; }
    }

    /// <summary>
    /// Uniform Block class represents the JSON structure of a Uniform Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class UniformBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the lower bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Low { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double High { get; set; }
    }

    /// <summary>
    /// Triangular Block class represents the JSON structure of a Triangular Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class TriangularBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the lower bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Low { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double High { get; set; }

        /// <summary>
        /// Gets or sets the mode of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Mode { get; set; }
    }

    /// <summary>
    /// Lognormal Block class represents the JSON structure of a Lognormal Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class LognormalBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the lower bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Low { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Scale { get; set; }

        /// <summary>
        /// Gets or sets the shape of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Shape { get; set; }
    }

    /// <summary>
    /// Beta Block class represents the JSON structure of a Beta Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class BetaBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the lower bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Low { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double High { get; set; }

        /// <summary>
        /// Gets or sets the Alpha value of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Shape1 { get; set; }

        /// <summary>
        /// Gets or sets the Beta value of the <see cref="Simphony.Mathematics"/> Distribution.
        /// </summary>
        required public double Shape2 { get; set; }
    }

    /// <summary>
    /// Normal Distribution Block class represents the JSON structure of a Normal Distribution.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class NormalDistBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the mean value of the <see cref="Simphony.Mathematics.Normal"/> Distribution.
        /// </summary>
        required public double Mean { get; set; }

        /// <summary>
        /// Gets or sets the variance value of the <see cref="Simphony.Mathematics.Normal"/> Distribution.
        /// </summary>
        required public double Variance { get; set; }
    }

    /// <summary>
    /// Resource block class representing the JSON structure for Resource Input.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class ResourceBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="ResourceBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ResourceType Type { get; set; }

        /// <summary>
        /// Gets or sets the number of units of the <see cref="ResourceBlock"/>.
        /// </summary>
        required public int NoOfUnit { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="ResourceBlock"/>.
        /// </summary>
        required public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="CostBlock"/> of the <see cref="ResourceBlock"/>.
        /// </summary>
        public List<CostBlock>? Cost { get; set; }
    }

    /// <summary>
    /// Cost block class representing the JSON structure for Resource Cost.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too many cluttered files otherwise.")]
    public class CostBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="CostBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ResourceCostType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="CostBlock"/>.
        /// </summary>
        required public double Value { get; set; }
    }
}
