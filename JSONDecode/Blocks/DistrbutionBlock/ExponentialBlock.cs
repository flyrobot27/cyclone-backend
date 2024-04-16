namespace CYCLONE.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Exponential Block class represents the JSON structure of an Exponential Distribution.
    /// </summary>
    public class ExponentialBlock : DistributionBlock
    {
        /// <summary>
        /// Gets or sets the mean value of the <see cref="Simphony.Mathematics.Exponential"/> Distribution.
        /// </summary>
        required public double Mean { get; set; }
    }
}
