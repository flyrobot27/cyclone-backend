namespace CYCLONE.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Lognormal Block class represents the JSON structure of a Lognormal Distribution.
    /// </summary>
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
}
