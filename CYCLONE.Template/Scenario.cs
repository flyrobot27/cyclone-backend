namespace CYCLONE.Template
{
    using System.Collections.Generic;
    using ConsoleTables;
    using CYCLONE.Template.Interfaces;
    using CYCLONE.Template.ResultContainers;
    using CYCLONE.Template.Types;
    using Simphony;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a Simphony simulation scenario for the CYCLONE model.
    /// </summary>
    public class Scenario : IScenario
    {
        private readonly DiscreteEventEngine engine;
        private readonly List<IElement<CycloneNetworkType>> elements = [];
        private readonly double length;
        private readonly int numberOfRuns;
        private readonly int seed;
        private readonly bool debug = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario"/> class.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="engine">The <see cref="DiscreteEventEngine"/>.</param>
        /// <param name="length">Maximum execution time of the model. If no limit is desired, set it to 0.</param>
        /// <param name="numberOfRuns">Number of simulations to run for Monte-Carlo simulation.</param>
        /// <param name="seed">Set randomization seed. Non-zero if reproducability is desired. 0 if fully randomize is desired.</param>
        /// <param name="debug">Set to true to enable debug mode.</param>
        public Scenario(string processName, DiscreteEventEngine engine, double length, int numberOfRuns = 1, int seed = 0, bool debug = false)
        {
            engine.ExceptionIfNull(nameof(engine));
            length.ExceptionIfNegative(nameof(length));
            numberOfRuns.ExceptionIfNotPositive(nameof(numberOfRuns));
            seed.ExceptionIfNegative(nameof(seed));

            this.engine = engine;
            this.length = length;
            this.numberOfRuns = numberOfRuns;
            this.seed = seed;
            this.debug = debug;
            this.ProcessName = processName;
        }

        /// <summary>
        /// Gets the non-intrinsic results.
        /// </summary>
        public Dictionary<string, NonIntrinsicResult> NonIntrinsicResults { get; } = [];

        /// <summary>
        /// Gets the intrinsic results.
        /// </summary>
        public Dictionary<string, IntrinsicResult> IntrinsicResults { get; } = [];

        /// <summary>
        /// Gets the counter results.
        /// </summary>
        public Dictionary<string, CounterResult> CounterResults { get; } = [];

        /// <summary>
        /// Gets the waiting file results.
        /// </summary>
        public Dictionary<string, WaitingFileResult> WaitingFileResults { get; } = [];

        /// <summary>
        /// Gets the termination time of the scenario.
        /// </summary>
        public NumericStatistic TerminationTime { get; } = new("TerminationTime", NumericStatisticInterpretation.FinishTime);

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        public string ProcessName { get; }

        /// <inheritdoc/>
        public double AbsoluteError => 1E-5;

        /// <inheritdoc/>
        public bool Enabled { get; set; } = true;

        /// <inheritdoc/>
        public double RelativeError => 1E-5;

        /// <inheritdoc/>
        public IEnumerable<IStateVariable> StateVariables { get; } = [];

        /// <inheritdoc/>
        public double TimeStep => 1;

        /// <inheritdoc/>
        public void F(double t, double[] y, double[] yp)
        {
            // Do nothing
        }

        /// <inheritdoc/>
        public void FinalizeRun(int runIndex)
        {
            foreach (var element in this.elements)
            {
                element.FinalizeRun(runIndex);
            }

            this.engine.CollectStatistic(this.TerminationTime, this.engine.TimeNow);
            this.TerminationTime.FinalizeRun(runIndex, this.engine.TimeNow);
        }

        /// <inheritdoc/>
        public void FinalizeScenario()
        {
            if (this.debug)
            {
                Console.WriteLine($"Scenario Finished at {this.engine.TimeNow}");
            }

            // Add Termination Time to the non-intrinsic results
            this.NonIntrinsicResults.Add(
                $"{this.ProcessName} ({this.TerminationTime.Name})",
                new NonIntrinsicResult(
                    this.TerminationTime.Minimum,
                    this.TerminationTime.Maximum,
                    this.TerminationTime.Mean,
                    this.TerminationTime.StandardDeviation,
                    this.TerminationTime.Observations.Count));

            foreach (var element in this.elements)
            {
                this.CollectStatistics(element);
                this.CollectCounterResult(element);
                this.CollectWaitingFileResult(element);
            }

            if (this.debug)
            {
                ConsoleTable consoleTables;
                Console.WriteLine("Intrinsic Results");
                consoleTables = new ConsoleTable("Element Name", "Mean", "Standard Deviation", "Minimum", "Maximum", "Current");
                foreach (var result in this.IntrinsicResults)
                {
                    var intrinsicResult = result.Value;
                    consoleTables.AddRow(
                        result.Key, 
                        intrinsicResult.Mean.ToString("N3"), 
                        intrinsicResult.StdDev.ToString("N3"), 
                        intrinsicResult.Min.ToString("N3"), 
                        intrinsicResult.Max.ToString("N3"), 
                        intrinsicResult.Current.ToString("N3"));
                }

                consoleTables.Write();

                Console.WriteLine("Non-Intrinsic Results");
                consoleTables = new ConsoleTable("Element Name", "Mean", "Standard Deviation", "Observations", "Minimum", "Maximum");
                foreach (var result in this.NonIntrinsicResults)
                {
                    var nonIntrinsicResult = result.Value;
                    consoleTables.AddRow(
                        result.Key, 
                        nonIntrinsicResult.Mean.ToString("N3"), 
                        nonIntrinsicResult.StdDev.ToString("N3"), 
                        nonIntrinsicResult.ObservationCount.ToString("N3"), 
                        nonIntrinsicResult.Min.ToString("N3"), 
                        nonIntrinsicResult.Max.ToString("N3"));
                }

                consoleTables.Write();
                
                Console.WriteLine("Counter Results");
                consoleTables = new ConsoleTable("Element Name", "Final Count", "Production Rate", "Average Inter-Arrival Time", "First Arrival", "Last Arrival");
                foreach (var result in this.CounterResults)
                {
                    var counterResult = result.Value;
                    consoleTables.AddRow(
                        result.Key, 
                        counterResult.FinalCount.ToString("N3"), 
                        counterResult.ProductionRate.ToString("N3"),
                        counterResult.AverageInterArrivalTime.ToString("N3"),
                        counterResult.FirstArrival.ToString("N3"),
                        counterResult.LastArrival.ToString("N3"));
                }

                consoleTables.Write();

                Console.WriteLine("Waiting File Results");
                consoleTables = new ConsoleTable("Element Name", "Average Length", "Standard Deviation", "Maximum Length", "Current Length", "Average Wait Time");
                foreach (var result in this.WaitingFileResults)
                {
                    var waitingFileResult = result.Value;
                    consoleTables.AddRow(
                        result.Key, 
                        waitingFileResult.AverageLength.ToString("N3"), 
                        waitingFileResult.StdDev.ToString("N3"), 
                        waitingFileResult.MaxLength.ToString("N3"), 
                        waitingFileResult.CurrentLength.ToString("N3"), 
                        waitingFileResult.AvgWaitTime.ToString("N3"));
                }

                consoleTables.Write();
            }
        }

        /// <inheritdoc/>
        public double InitializeRun(int runIndex)
        {
            this.elements.Count.ExceptionIfNotPositive(
                nameof(this.elements.Count),
                string.Format("No Element has been inserted. Use {0} to insert element into the Scenario.", nameof(this.InsertElements)));

            foreach (var element in this.elements)
            {
                element.SetDiscreteEventEngine(this.engine);
                element.Debug = this.debug;
                element.InitializeRun(runIndex);
            }

            this.TerminationTime.InitializeRun(runIndex);
            return this.length;
        }

        /// <inheritdoc/>
        public int InitializeScenario()
        {
            if (this.seed == 0)
            {
                Distribution.Seed();
            }
            else
            {
                Distribution.Seed(this.seed);
            }

            return this.numberOfRuns;
        }

        /// <summary>
        /// Inserts <see cref="IElement{T}"/> into the scenario.
        /// </summary>
        /// <param name="elements">The element to insert.</param>
        public void InsertElements(params IElement<CycloneNetworkType>[] elements)
        {
            if (this.debug)
            {
                foreach (var element in elements)
                {
                    Console.WriteLine(element.ToString());
                    Console.WriteLine("=====================================");
                    element.Debug = true;
                }
            }

            this.elements.AddRange(elements);
        }

        private void CollectStatistics(IElement<CycloneNetworkType> element)
        {
            if (element is Counter)
            {
                return;
            }

            var statisitcs = element.GetStatistics();

            foreach (var stats in statisitcs)
            {
                if (stats is NumericStatistic numericStats)
                {
                    var resultString = $"{element.Label} - {element.Description} ({numericStats.Name})";
                    if (numericStats.IsIntrinsic)
                    {
                        this.IntrinsicResults.Add(resultString, new IntrinsicResult(numericStats.Minimum, numericStats.Maximum, numericStats.Mean, numericStats.StandardDeviation, numericStats.Current));
                    }
                    else
                    {
                        this.NonIntrinsicResults.Add(resultString, new NonIntrinsicResult(numericStats.Minimum, numericStats.Maximum, numericStats.Mean, numericStats.StandardDeviation, numericStats.Observations.Count));
                    }
                }
            }
        }

        private void CollectCounterResult(IElement<CycloneNetworkType> element)
        {
            if (element is not Counter counter)
            {
                return;
            }

            var resultString = $"{element.Label} - {element.Description}";

            var result = new CounterResult(
                counter.LastCount.Mean, 
                counter.ProductionRate.Observations[counter.ProductionRate.Observations.Count - 1], 
                counter.InterArrivalTime.Mean, 
                counter.FirstTime.Mean, 
                counter.LastTime.Mean);

            this.CounterResults.Add(resultString, result);
        }

        private void CollectWaitingFileResult(IElement<CycloneNetworkType> element)
        {
            var waitingFiles = element.GetWaitingFiles();
            foreach ( var waitingFile in waitingFiles )
            {
                var fileLength = waitingFile.FileLength;
                var waitingTime = waitingFile.WaitingTime;

                var resultString = $"{element.Label} - {element.Description}";

                var result = new WaitingFileResult(
                    fileLength.Mean,
                    fileLength.StandardDeviation,
                    fileLength.Maximum,
                    fileLength.Current,
                    waitingTime.Mean);

                this.WaitingFileResults.Add(resultString, result);
            }
        }
    }
}
