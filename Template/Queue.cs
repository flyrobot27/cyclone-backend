namespace CYCLONE.Template
{
	using CYCLONE.Template.Model.Element;
	using Simphony;
	using Simphony.Simulation;

	public class Queue : ElementFlow
	{
		private static IList<Combi> CombiList = [];
		private int InitialLengthValue = 0;
		private static bool ScanTriggered = false;
		private WaitingFile InnerQueue = new WaitingFile();

		public NumericStatistic PercentNonempty = new NumericStatistic("PercentNonempty", true);

		public Queue(string id, string? description, int initialLength = 0) :
			base(id, description)
		{
			this.AddWaitingFile(InnerQueue);
			initialLength.ExceptionIfNegative(nameof(initialLength));
			this.InitialLengthValue = initialLength;
		}

		public int GetCurrentLength()
		{
			return this.InnerQueue.Count;
		}

		public Entity Dequeue()
		{
			var entity = this.Engine.DequeueEntity<Entity>(this.InnerQueue);
			this.Engine.CollectStatistic(this.PercentNonempty, Math.Sign(this.InnerQueue.Count));
			this.WriteDebugMessage(entity, "Departed");
			return entity;
		}

		private static void Scan(Entity entity)
		{
			foreach (var combi in CombiList)
			{
				if (combi == null) continue;
				while ((combi as Combi).TryExecute())
				{
					// do nothing
					continue;
				}
			}

			ScanTriggered = false;
		}

		private void InitializeLength(Entity entity)
		{
			this.WriteDebugMessage(entity, string.Format("Initialized to length {0}", this.InitialLengthValue));

			for (int i = 0; i < this.InitialLengthValue; i++)
			{
				this.Engine.EnqueueEntity(new Entity(), this.InnerQueue);
			}

			this.Engine.CollectStatistic(this.PercentNonempty, Math.Sign(this.InnerQueue.Count));

			if (!ScanTriggered)
			{
				this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(Scan), 0);
			}
		}

		public override void InitializeRun(int runIndex)
		{
			base.InitializeRun(runIndex);
			ScanTriggered = false;

			CombiList.ExceptionIfNullOrEmpty(nameof(CombiList));

			if (this.InitialLengthValue > 0)
			{
				this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(this.InitializeLength), 0);
			}
		}

		public override void TransferIn(Entity entity)
		{
			this.WriteDebugMessage(entity, "Arrived");
			this.Engine.EnqueueEntity(entity, this.InnerQueue);
			this.Engine.CollectStatistic(this.PercentNonempty, Math.Sign(this.InnerQueue.Count));

			if (!ScanTriggered)
			{
				this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(Scan), 0);
				ScanTriggered = true;
			}
		}
	}
}
