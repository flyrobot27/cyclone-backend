namespace CYCLONE.Template
{
    using CYCLONE.Template.Interfaces;
    using CYCLONE.Template.Types;
    using Simphony.Mathematics;
    using Simphony.Simulation;
    using System.Text;

    /// <summary>
    /// Represents a COMBI element in the CYCLONE model.
    /// </summary>
    /// <param name="label">The label of the element. Must be unique across all elements.</param>
    /// <param name="description">The description of the element.</param>
    /// <param name="duration">The distribution representing the delay duration.</param>
    public class Combi(string label, string description, Distribution duration)
        : Normal(label, description, duration, CycloneNetworkType.COMBI), IAddPreceders<CycloneNetworkType>
    {
        private readonly IList<Queue> queueList = [];

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

        /// <summary>
        /// Add preceding queue(s) to the combi.
        /// </summary>
        /// <param name="elements">The preceding <see cref="Queue"/>.</param>
        public void AddPreceders(params IElement<CycloneNetworkType>[] elements)
        {
            foreach (var element in elements)
            {
                if (element is not Queue queue)
                {
                    throw new InvalidOperationException("Only Queue elements can be added as preceders to a Combi element.");
                }

                this.queueList.Add(queue);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var baseString = base.ToString();
            var sb = new StringBuilder(baseString);
            sb.AppendLine("Preceders:");
            foreach (var queue in this.queueList)
            {
                sb.AppendLine($"  {queue.ToString()}");
            }
            sb.AppendLine("Followers:");
            foreach (var follower in this.Followers)
            {
                sb.AppendLine($"  {follower.ToString()}");
            }
            return sb.ToString();
        }
    }
}
