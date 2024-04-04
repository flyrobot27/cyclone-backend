using Simphony.Simulation;

namespace CYCLONE.Template.Model.Element
{
	public abstract class ElementFlow(string id, string? description) : ElementBase(id, description), IElement
	{
		public abstract void TransferIn(Entity entity);
	}
}
