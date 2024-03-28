namespace CYCLONE.Template
{
	using CYCLONE.Template.Model;
	using Simphony.Mathematics;
	using Simphony.Simulation;
	using System.Collections.Generic;

	public class Normal: ElementTask<Entity>
	{
		private double LastTime;
		private Boolean FirstEntity = false;

		public NumericStatistic InterArrivalTime = new NumericStatistic("InterArrivalTime", false);
		public Normal(Distribution duration, List<Element<Entity>> followers) :
			base(duration, followers)
		{
			
		}

		protected void InitializeRun()
		{

		}

	    protected override void TransferIn(Entity entity)
		{
			if (!this.FirstEntity)
			{
				this.Engine.CollectStatistic(this.InterArrivalTime, this.Engine.TimeNow - this.LastTime);
			}
			else
			{
				this.FirstEntity = false;
			}
			this.LastTime = this.Engine.TimeNow;


		}
	}
}
