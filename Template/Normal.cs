namespace CYCLONE.Template
{
	using CYCLONE.Template.Model.Element;
	using Simphony.Mathematics;
    using Simphony.Simulation;
    using System.Collections.Generic;

    public class Normal: ElementTask
	{
		private double LastTime;
		private Boolean FirstEntity = false;

		public NumericStatistic InterArrivalTime = new("InterArrivalTime", false);

		public Normal(string id, string description, Distribution duration, IList<IElement> followers) :
			base(id, description, duration, followers)
		{
			this.AddStatistics(this.InterArrivalTime);
		}

		public override void InitializeRun(int runIndex)
		{
			base.InitializeRun(runIndex);
			this.LastTime = double.NaN;
			this.FirstEntity = true;
		}

		public override void OnTransferOut(Entity entity)
		{
			this.WriteDebugMessage(entity, "Departed");
			base.OnTransferOut(entity);
		}

		public override void TransferIn(Entity entity)
		{
			this.WriteDebugMessage(entity, "Arrived");
			if (!this.FirstEntity)
			{
				this.Engine.CollectStatistic(this.InterArrivalTime, this.Engine.TimeNow - this.LastTime);
			}
			else
			{
				this.FirstEntity = false;
			}
			this.LastTime = this.Engine.TimeNow;
			base.TransferIn(entity);
		}
	}
}
