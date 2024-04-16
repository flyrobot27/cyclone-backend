namespace CYCLONE.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Deterministic Block class represents the JSON structure of a Deterministic Distribution.
    /// </summary>
    public class DeterministicBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the constant value of the <see cref="Simphony.Mathematics.Constant"/> Distribution.
        /// </summary>
        required public double Constant { get; set; }
    }
}
