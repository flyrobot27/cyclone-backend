namespace CYCLONE.Template.Interfaces
{
    using Simphony.Simulation;

    /// <summary>
    /// Interface for an element.
    /// </summary>
    /// <typeparam name="T">The Type to distinguish the type of the element.</typeparam>
    public interface IElement<T>
        where T : Enum
    {
        /// <summary>
        /// Gets the element type.
        /// </summary>
        T ElementType { get; }

        /// <summary>
        /// Abstract method that defines the behavior when an entity is transferred in.
        /// </summary>
        /// <param name="entity">The entity to be transferred in.</param>
        void TransferIn(Entity entity);

        /// <summary>
        /// Initialize the element for a simulation run.
        /// </summary>
        /// <param name="runIndex">The run index. Useful when using Monte-Carlo Simulation. If only a single run, set runIndex to 0.</param>
        void InitializeRun(int runIndex);

        /// <summary>
        /// Finalizes the element for a simulation run.
        /// </summary>
        /// <param name="runIndex">The run index. Useful when using Monte-Carlo Simulation. If only a single run, set runIndex to 0.</param>
        void FinalizeRun(int runIndex);

        /// <summary>
        /// Sets the Discrete Event Engine.
        /// </summary>
        /// <param name="engine">The Discrete Event Engine.</param>
        void SetDiscreteEventEngine(DiscreteEventEngine engine);
    }
}
