namespace CYCLONE.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Triangular Block class represents the JSON structure of a Triangular Distribution.
    /// </summary>
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
}
