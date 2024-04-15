namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Model.Exception;

    using Simphony;
    using Simphony.Mathematics;
    using Simphony.Simulation;
    using System.Collections.Generic;

    public class Scenario : IScenario
    {
        private readonly DiscreteEventEngine engine;
        private readonly List<IElement> elements = [];
        private readonly double length;
        private readonly int numberOfRuns;
        private readonly int terminationCount;
        private readonly int seed;

        public Scenario(DiscreteEventEngine engine, double length, int terminationCount, int numberOfRuns = 1, int seed = 0)
        {
            engine.ExceptionIfNull(nameof(engine));
            length.ExceptionIfNull(nameof(length));
            this.engine = engine;
            this.length = length;
            this.numberOfRuns = numberOfRuns;
            this.terminationCount = terminationCount;
            this.seed = seed;
        }

        public double AbsoluteError => 1E-5;

        public bool Enabled { get; set; } = true;

        public double RelativeError => 1E-5;

        public IEnumerable<IStateVariable> StateVariables { get; } = [];

        public double TimeStep => 1;

        public void F(double t, double[] y, double[] yp)
        {
            // Do nothing
        }

        public void FinalizeRun(int runIndex)
        {
            foreach (var element in this.elements)
            {
                element.FinalizeRun(runIndex);
            }
        }

        public void FinalizeScenario()
        {
            // Do nothing
        }

        public double InitializeRun(int runIndex)
        {
            this.elements.Count.ExceptionIfNotPositive(nameof(this.elements.Count));

            // Todo: Initialize Queues
            foreach (var element in this.elements)
            {
                element.SetDiscreteEventEngine(this.engine);
                element.InitializeRun(runIndex);
            }

            return this.length;
        }

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

        public void InsertElement(IElement element)
        {
            this.elements.Add(element);
        }
    }
}
