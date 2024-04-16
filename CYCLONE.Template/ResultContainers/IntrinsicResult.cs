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
        /// Gets the Minimum Value Observed.
        /// </summary>
        public double Min { get; } = min;

        /// <summary>
        /// Gets the Maximum Value Observed.
        /// </summary>
        public double Max { get; } = max;

        /// <summary>
        /// Gets the Average Value Observed.
        /// </summary>
        public double Mean { get; } = mean;

        /// <summary>
        /// Gets the Standard Deviation.
        /// </summary>
        public double StdDev { get; } = stdDev;

        /// <summary>
        /// Gets the Current Value.
        /// </summary>
        public double Current { get; } = current;
    }
}
