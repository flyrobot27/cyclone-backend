namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;
    using Simphony.ComponentModel;
    using Simphony.Simulation;

    public class Counter : ElementFunction
	{
		private bool seenNone;

		public NumericStatistic FirstTime { get; }
		public NumericStatistic InterArrivalTime { get; }
		public NumericStatistic LastCount { get; }
		public NumericStatistic LastTime { get; }
		public NumericStatistic Production { get; }
		public NumericStatistic ProductionRate { get; }
		public int Limit { get; set; } = 0;
		public int Step { get; set; } = 1;
		public int Initial { get; set; } = 0;
		public int Count { get; private set; }

		public double First { get; private set; } = double.NaN;
		public double Last { get; private set; } = double.NaN;

		public Counter(string label, string? description, IList<IElement> followers) : 
			base(label, description, followers, NetworkType.FUNCTION_COUNTER)
		{
			this.FirstTime = new NumericStatistic("FirstTime", false);
			this.InterArrivalTime = new NumericStatistic("InterArrivalTime", NumericStatisticInterpretation.InterarrivalTime);
			this.LastCount = new NumericStatistic("LastCount", false);
			this.LastTime = new NumericStatistic("LastTime", false);
			this.Production = new NumericStatistic("Production", NumericStatisticInterpretation.OverallProduction);
			this.ProductionRate = new NumericStatistic("ProductionRate", NumericStatisticInterpretation.ProductionRate);

			this.AddStatistics(this.FirstTime, this.InterArrivalTime, this.LastCount, this.LastTime, this.Production, this.ProductionRate);
		}

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

		public override void FinalizeRun(int runIndex)
		{
			this.Engine.CollectStatistic(this.LastCount, this.Count);
			this.Engine.CollectStatistic(this.LastTime, this.Last);
			base.FinalizeRun(runIndex);
		}

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
