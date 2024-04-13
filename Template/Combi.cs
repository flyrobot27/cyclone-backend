namespace CYCLONE.Template
{
	using CYCLONE.Template.Model.Element;
	using Simphony.Mathematics;
	using Simphony.Simulation;

	public class Combi(string id, string description, Distribution duration, IList<IElement> followers, IList<Queue> preceders) : 
		Normal(id, description, duration, followers, NetworkType.COMBI)
	{
		private readonly IList<Queue> QueueList = preceders;

		public bool TryExecute()
		{
			// Detect for empty queue
			foreach (var queue in QueueList)
			{
				if (queue.GetCurrentLength() == 0)
				{
					return false;
				}
			}

			// detect for null entity
			Entity? entity = null;
			foreach (var queue in QueueList)
			{
				entity = queue.Dequeue();
			}
			if (entity == null)
			{
				return false;
			}

			// transfer entity
			base.TransferIn(entity);
			return true;
		}
	}
}
