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
        /// Gets the final items counted at the counter.
        /// </summary>
        public double FinalCount { get; } = finalCount;
        
        /// <summary>
        /// Gets the steady state rate of production.
        /// </summary>
        public double ProductionRate { get; } = productionRate;

        /// <summary>
        /// Gets the average inter-arrival time.
        /// </summary>
        public double AverageInterArrivalTime { get; } = averageInterArrivalTime;

        /// <summary>
        /// Gets the time of the first arrival.
        /// </summary>
        public double FirstArrival { get; } = firstArrival;

        /// <summary>
        /// Gets the time of the last arrival.
        /// </summary>
        public double LastArrival { get; } = lastArrival;
    }
}
