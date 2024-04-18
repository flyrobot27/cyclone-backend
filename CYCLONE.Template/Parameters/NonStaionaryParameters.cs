namespace CYCLONE.Template.Parameters
{
    using Simphony.Mathematics;

    /// <summary>
    /// Represents the parameters for a non-stationary input.
    /// </summary>
    /// <param name="increment">Number to increment the low and high of a <see cref="Distribution"/>.</param>
    /// <param name="realizationNumber">Number of cycles which the increment will happen.</param>
    /// <param name="seed">Randomization Seed.</param>
    public class NonStaionaryParameters(double increment, int realizationNumber, int seed = 0)
    {
        /// <summary>
        /// Gets the increment value.
        /// </summary>
        public double Increment { get; } = increment;

        /// <summary>
        /// Gets the number of cycles which the increment will happen.
        /// </summary>
        public int RealizationNumber { get; } = realizationNumber;

        /// <summary>
        /// Gets the randomization seed.
        /// </summary>
        public int Seed { get; } = seed;
    }
}
