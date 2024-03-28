namespace CYCLONE.Template.Model
{
	using Simphony.Mathematics;
	using Simphony.Simulation;
	using System.ComponentModel.Design.Serialization;

	public abstract class ElementTask<T>(Distribution duration, List<Element<T>> targetElements) : Element<T> where T: Entity
	{
		protected DiscreteEventEngine Engine = new();
		protected Distribution Duration = duration;
		protected List<Element<T>> TargetElements = targetElements;

		protected virtual void OnTransferOut(T entity)
		{
			if (this.Engine.CurrentEntity != entity)
			{
				this.Engine.ScheduleEvent(entity, this.OnTransferOut, 0D);
				return;
			}
		}

		protected override void TransferIn(T entity)
		{
			try
			{
				var handler = new Action<T>(this.OnTransferOut);
				this.Engine.ScheduleEvent(entity, handler, this.Duration.Sample());
			}
			catch (ModelExecutionException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ModelExecutionException(ex, this);
			}
		}
	}
}
