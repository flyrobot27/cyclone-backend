namespace CYCLONE.Template.Model.Element
{
    using CYCLONE.Template.Interfaces;
    using CYCLONE.Types;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a function element in the CYCLONE model.
    /// </summary>
    /// <param name="label">The label of the element. Must be unique across all elements.</param>
    /// <param name="description">The description of the element.</param>
    /// <param name="type">The <see cref="CycloneNetworkType"/> of the element.</param>
    public class ElementFunction(string label, string? description, CycloneNetworkType type)
        : CycloneElementBase(label, description, type), IAddFollowers<CycloneNetworkType>
    {
        private readonly IList<IElement<CycloneNetworkType>> followers = [];

        /// <inheritdoc/>
        public override void TransferIn(Entity entity)
        {
            for (int i = 0; i < this.followers.Count; i++)
            {
                this.followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }

        /// <inheritdoc/>
        public void AddFollowers(IElement<CycloneNetworkType>[] elements)
        {
            foreach (var element in elements)
            {
                this.followers.Add(element);
            }
        }
    }
}
