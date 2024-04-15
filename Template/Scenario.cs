namespace CYCLONE.Template
{
    using System.Collections.Generic;

    using CYCLONE.Template.Interfaces;
    using CYCLONE.Types;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario"/> class.
        /// </summary>
        /// <param name="engine">The <see cref="DiscreteEventEngine"/>.</param>
        /// <param name="length">Maximum execution time of the model. If no limit is desired, set it to 0.</param>
        /// <param name="numberOfRuns">Number of simulations to run for Monte-Carlo simulation.</param>
        /// <param name="seed">Set randomization seed. Non-zero if reproducability is desired. 0 if fully randomize is desired.</param>
        public Scenario(DiscreteEventEngine engine, double length, int numberOfRuns = 1, int seed = 0)
        {
            engine.ExceptionIfNull(nameof(engine));
            length.ExceptionIfNegative(nameof(length));
            numberOfRuns.ExceptionIfNotPositive(nameof(numberOfRuns));
            seed.ExceptionIfNegative(nameof(seed));

            this.engine = engine;
            this.length = length;
            this.numberOfRuns = numberOfRuns;
            this.seed = seed;
        }

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
        }

        /// <inheritdoc/>
        public void FinalizeScenario()
        {
            // Do nothing
        }

        /// <inheritdoc/>
        public double InitializeRun(int runIndex)
        {
            this.elements.Count.ExceptionIfNotPositive(
                nameof(this.elements.Count), 
                string.Format("No Element has been inserted. Use {0} to insert element into the Scenario.", nameof(this.InsertElement)));

            foreach (var element in this.elements)
            {
                element.SetDiscreteEventEngine(this.engine);
                element.InitializeRun(runIndex);
            }

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
        /// Inserts an element into the scenario.
        /// </summary>
        /// <param name="element">The element to insert.</param>
        public void InsertElement(IElement<CycloneNetworkType> element)
        {
            this.elements.Add(element);
        }
    }
}
