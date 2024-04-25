namespace CYCLONE.API.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Uniform Block class represents the JSON structure of a Uniform Distribution.
    /// </summary>
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
}
