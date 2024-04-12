namespace CYCLONE.Template
{
	using CYCLONE.Template.Model;
	using CYCLONE.Template.Model.Element;
	using Simphony.Mathematics;
	using Simphony.Simulation;

	public class Combi(string id, string description, Distribution duration, IList<IElement> followers, IList<Queue> preceders) : Normal(id, description, duration, followers)
	{
		private readonly IList<Queue> QueueList = preceders;

		public bool TryExecute()
		{
			foreach(var queue in QueueList)
			{
				if (queue.GetCurrentLength() == 0)
				{
					return false;
				}
			}

			Entity? entity = null;
			foreach(var queue in QueueList)
			{
				entity = queue.Dequeue();
			}
			
			// transfer in
			if (entity == null)
			{
				throw new ModelExecutionException("Must be preceded by queues");
			}
			this.Engine.ScheduleEvent(entity, this.TransferToElement, 0);
			return true;
		}

		private void TransferToElement(Entity entity) 
		{
			foreach (var e in this.Followers)
			{
				e.TransferIn(entity);
			}
		}
	}
}
