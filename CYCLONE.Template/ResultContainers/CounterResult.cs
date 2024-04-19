namespace CYCLONE.Template.ResultContainers
{
    using CYCLONE.Template;

    /// <summary>
    /// Represents the result of a <see cref="Counter"/>.
    /// </summary>
    /// <param name="finalCount">Final items counted at the counter.</param>
    /// <param name="productionRate">The steady state rate of production.</param>
    /// <param name="averageInterArrivalTime">Average inter-arrival time.</param>
    /// <param name="firstArrival">Time of the first arrival.</param>
    /// <param name="lastArrival">Time of the last arrival.</param>
    public class CounterResult(double finalCount, double productionRate, double averageInterArrivalTime, double firstArrival, double lastArrival)
    {
        /// <summary>
        /// Gets the final items counted at the counter as a string.
        /// </summary>
        public string FinalCount { get; } = finalCount.ToString("N3");
        
        /// <summary>
        /// Gets the steady state rate of production as a string
        /// </summary>
        public string ProductionRate { get; } = productionRate.ToString("N3");

        /// <summary>
        /// Gets the average inter-arrival time as a string
        /// </summary>
        public string AverageInterArrivalTime { get; } = averageInterArrivalTime.ToString("N3");

        /// <summary>
        /// Gets the time of the first arrival as a string
        /// </summary>
        public string FirstArrival { get; } = firstArrival.ToString("N3");

        /// <summary>
        /// Gets the time of the last arrival as a string
        /// </summary>
        public string LastArrival { get; } = lastArrival.ToString("N3");
    }
}
