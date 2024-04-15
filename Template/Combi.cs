namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a COMBI element in the CYCLONE model.
    /// </summary>
    /// <param name="label">The label of the element. Must be unique across all elements.</param>
    /// <param name="description">The description of the element.</param>
    /// <param name="duration">The distribution representing the delay duration.</param>
    /// <param name="followers">The element following the Combi.</param>
    /// <param name="preceders">The Queues preceding the Combi.</param>
    public class Combi(string label, string description, Distribution duration, IList<IElement> followers, IList<Queue> preceders)
        : Normal(label, description, duration, followers, NetworkType.COMBI)
    {
        private readonly IList<Queue> queueList = preceders;

        /// <summary>
        /// Try to execute the combi. Will Try to dequeue from all queues and transfer the entity to the followers.
        /// </summary>
        /// <returns>True if successful. False otherwise.</returns>
        public bool TryExecute()
        {
            // Detect for empty queue
            foreach (var queue in this.queueList)
            {
                if (queue.GetCurrentLength() == 0)
                {
                    return false;
                }
            }

            // detect for null entity
            Entity? entity = null;
            foreach (var queue in this.queueList)
            {
                entity = queue.Dequeue();
            }

            if (entity == null)
            {
                return false;
            }

            // transfer entity
            this.TransferIn(entity);
            return true;
        }
    }
}
