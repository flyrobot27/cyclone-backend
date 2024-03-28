namespace CYCLONE.Template.Model
{
	using Simphony.Simulation;

	public abstract class Element<T> where T: Entity
	{
		protected abstract void TransferIn(T entity);
	}
}
