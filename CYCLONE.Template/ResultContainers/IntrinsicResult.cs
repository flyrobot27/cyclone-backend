namespace CYCLONE.Template.ResultContainers
{
    using Simphony.Simulation;

    /// <summary>
    /// Represents the result of an intrinsic <see cref="Statistic"/>.
    /// </summary>
    /// <param name="min">Minimum Value Observed.</param>
    /// <param name="max">Maximum Value Observed</param>
    /// <param name="mean">Average Value Observed.</param>
    /// <param name="stdDev">Standard Deviation.</param>
    /// <param name="current">Current Value.</param>
    public class IntrinsicResult(double min, double max, double mean, double stdDev, double current)
    {
        /// <summary>
        /// Gets the Minimum Value Observed as a string
        /// </summary>
        public string Min { get; } = min.ToString("N3");

        /// <summary>
        /// Gets the Maximum Value Observed as a string.
        /// </summary>
        public string Max { get; } = max.ToString("N3");

        /// <summary>
        /// Gets the Average Value Observed as a string.
        /// </summary>
        public string Mean { get; } = mean.ToString("N3");

        /// <summary>
        /// Gets the Standard Deviation as a string.
        /// </summary>
        public string StdDev { get; } = stdDev.ToString("N3");

        /// <summary>
        /// Gets the Current Value as a string.
        /// </summary>
        public string Current { get; } = current.ToString("N3");
    }
}
