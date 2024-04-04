namespace CYCLONE.Template.Model.Element
{
    using Simphony.Mathematics;
    using Simphony.Simulation;

    public abstract class ElementTask(string id, string description, Distribution duration, IList<IElement> followers) : ElementBase(id, description), IElement
	{
        protected Distribution Duration = duration;
        protected IList<IElement> Followers = followers;

        public virtual void OnTransferOut(Entity entity)
        {
            if (Engine.CurrentEntity != entity)
            {
                Engine.ScheduleEvent(entity, OnTransferOut, 0D);
                return;
            }

            for (int i = 0; i < Followers.Count; i++)
            {
                Followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }

        public virtual void TransferIn(Entity entity)
        {
            try
            {
                var handler = new Action<Entity>(OnTransferOut);
                Engine.ScheduleEvent(entity, handler, Duration.Sample());
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
