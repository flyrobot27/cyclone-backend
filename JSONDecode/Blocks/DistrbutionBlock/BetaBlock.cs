namespace CYCLONE.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Beta Block class represents the JSON structure of a Beta Distribution.
    /// </summary>
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
}
