namespace CYCLONE.API.JSONDecode.Blocks.DistrbutionBlock
{
    /// <summary>
    /// Normal Distribution Block class represents the JSON structure of a Normal Distribution.
    /// </summary>
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
}
