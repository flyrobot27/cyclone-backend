namespace CYCLONE.Template.Model.Element
{
    using CYCLONE.Types;
    using Simphony.Simulation;

    public class ElementFunction(string label, string? description, IList<IElement> followers, NetworkType type) : ElementBase(label, description, type), IElement
    {
        protected IList<IElement> followers = followers;

        public override void TransferIn(Entity entity)
        {
            for (int i = 0; i < this.followers.Count; i++)
            {
                this.followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }
    }
}
