namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;
    using Simphony.ComponentModel;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a Counter element in the CYCLONE model.
    /// </summary>
    public class Counter : ElementFunction
    {
        private bool seenNone;

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class.
        /// </summary>
        /// <param name="label">The label of the element. Must be unique across all elements.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="followers">The elements following the Counter.</param>
        public Counter(string label, string? description, IList<IElement> followers)
            : base(label, description, followers, NetworkType.FUNCTION_COUNTER)
        {
            this.FirstTime = new NumericStatistic("FirstTime", false);
            this.InterArrivalTime = new NumericStatistic("InterArrivalTime", NumericStatisticInterpretation.InterarrivalTime);
            this.LastCount = new NumericStatistic("LastCount", false);
            this.LastTime = new NumericStatistic("LastTime", false);
            this.Production = new NumericStatistic("Production", NumericStatisticInterpretation.OverallProduction);
            this.ProductionRate = new NumericStatistic("ProductionRate", NumericStatisticInterpretation.ProductionRate);

            this.AddStatistics(this.FirstTime, this.InterArrivalTime, this.LastCount, this.LastTime, this.Production, this.ProductionRate);
        }

        /// <summary>
        /// Gets a statistic describing the time at which the first entity was observed.
        /// </summary>
        public NumericStatistic FirstTime { get; }

        /// <summary>
        /// Gets a statistic describing the time between entities arriving at the element.
        /// </summary>
        public NumericStatistic InterArrivalTime { get; }

        /// <summary>
        /// Gets a statistic describing the count when the most recent entity was observed.
        /// </summary>
        public NumericStatistic LastCount { get; }

        /// <summary>
        /// Gets a statistic describing the time at which the most recent entity was observed.
        /// </summary>
        public NumericStatistic LastTime { get; }

        /// <summary>
        /// Gets a statistic describing the overall production.
        /// </summary>
        public NumericStatistic Production { get; }

        /// <summary>
        /// Gets a statistic describing the ratio of the count and simulation time.
        /// </summary>
        public NumericStatistic ProductionRate { get; }

        /// <summary>
        /// Gets or sets the limit of the counter. If the limit is reached, the simulation will halt.
        /// </summary>
        public int Limit { get; set; } = 0;

        /// <summary>
        /// Gets or sets the step of the counter.
        /// </summary>
        public int Step { get; set; } = 1;

        /// <summary>
        /// Gets or sets the initial count of the counter.
        /// </summary>
        public int Initial { get; set; } = 0;

        /// <summary>
        /// Gets the current count.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the time at which the first entity was observed.
        /// </summary>
        public double First { get; private set; } = double.NaN;

        /// <summary>
        /// Gets the time at which the most recent entity was observed.
        /// </summary>
        public double Last { get; private set; } = double.NaN;

        /// <inheritdoc/>
        public override void InitializeRun(int runIndex)
        {
            this.Count = this.Initial;
            this.First = double.NaN;
            this.Last = double.NaN;
            this.Production.Initial = this.Initial;
            this.ProductionRate.Initial = 0D;
            this.seenNone = true;

            base.InitializeRun(runIndex);
        }

        /// <inheritdoc/>
        public override void FinalizeRun(int runIndex)
        {
            this.Engine.CollectStatistic(this.LastCount, this.Count);
            this.Engine.CollectStatistic(this.LastTime, this.Last);
            base.FinalizeRun(runIndex);
        }

        /// <inheritdoc/>
        public override void TransferIn(Entity entity)
        {
            this.WriteDebugMessage(entity, "Arrived at Counter");
            this.Count += this.Step;

            var converter = new RoundingConverter();
            this.WriteDebugMessage(entity, string.Format("Count = {0}", converter.ConvertToString(this.Count)));

            if (this.seenNone)
            {
                this.First = this.Engine.TimeNow;
                this.Engine.CollectStatistic(this.FirstTime, this.First);
                this.seenNone = false;
            }
            else
            {
                this.Engine.CollectStatistic(this.InterArrivalTime, this.Engine.TimeNow - this.Last);
            }

            this.Last = this.Engine.TimeNow;
            this.Engine.CollectStatistic(this.Production, this.Count);

            if (this.Engine.TimeNow > 0D)
            {
                this.Engine.CollectStatistic(this.ProductionRate, this.Count / this.Engine.TimeNow);
            }

            if (this.Limit > 0 && this.Count >= this.Limit)
            {
                this.Engine.HaltRun();
                this.WriteDebugMessage(entity, "Counter Limit Reached");
                return;
            }

            base.TransferIn(entity);
            this.WriteDebugMessage(entity, "Departed");
        }
    }
}
