namespace CYCLONE.Template.ResultContainers
{
    using Simphony.Simulation;

    /// <summary>
    /// Represents the result of a <see cref="WaitingFile"/>.
    /// </summary>
    /// <param name="averageLength">Average number of <see cref="Entity"/> in the <see cref="WaitingFile"/.></param>
    /// <param name="stdDev">Standard Deviation of the <paramref name="averageLength"/>.</param>
    /// <param name="maxLength">Maximum number of <see cref="Entity"/> in the <see cref="WaitingFile"/>.</param>
    /// <param name="currentLength">Current Number of <see cref="Entity"/> in the <see cref="WaitingFile"/>. </param>
    /// <param name="avgWaitTime">Average Waiting Time for an <see cref="Entity"/> before it is dequeued from the <see cref="WaitingFile"/>.</param>
    public class WaitingFileResult(double averageLength, double stdDev, double maxLength, double currentLength, double avgWaitTime)
    {
        /// <summary>
        /// Gets the Average number of <see cref="Entity"/> in the <see cref="WaitingFile"/>.
        /// </summary>
        public string AverageLength { get; } = averageLength.ToString("N3");

        /// <summary>
        /// Gets the Standard Deviation of the <see cref="AverageLength"/>.
        /// </summary>
        public string StdDev { get; } = stdDev.ToString("N3");

        /// <summary>
        /// Gets the Maximum number of <see cref="Entity"/> in the <see cref="WaitingFile"/>.
        /// </summary>
        public string MaxLength { get; } = maxLength.ToString("N3");

        /// <summary>
        /// Gets the Current Number of <see cref="Entity"/> in the <see cref="WaitingFile"/>.
        /// </summary>
        public string CurrentLength { get; } = currentLength.ToString("N3");

        /// <summary>
        /// Gets the Average Waiting Time for an <see cref="Entity"/> before it is dequeued from the <see cref="WaitingFile"/>.
        /// </summary>
        public string AvgWaitTime { get; } = avgWaitTime.ToString("N3");
    }
}
