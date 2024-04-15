namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;
    using Simphony;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a QUEUE element in the CYCLONE model.
    /// </summary>
    public class Queue : ElementBase, IElement
    {
        private static readonly IList<Combi> CombiList = [];
        private static bool scanTriggered = false;

        private readonly int initialLengthValue = 0;
        private readonly WaitingFile innerQueue = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue"/> class.
        /// </summary>
        /// <param name="label">The unique label of the queue. Must be unique across all elements.</param>
        /// <param name="description">The description of the queue.</param>
        /// <param name="initialLength">The initial length of the queue.</param>
        public Queue(string label, string? description, int initialLength = 0)
            : base(label, description, NetworkType.QUEUE)
        {
            initialLength.ExceptionIfNegative(nameof(initialLength));
            this.AddWaitingFile(this.innerQueue);
            this.AddStatistics(this.PercentNonempty);

            this.initialLengthValue = initialLength;
        }

        /// <summary>
        /// Gets the numeric statistics of the queue.
        /// </summary>
        public NumericStatistic PercentNonempty { get; } = new ("PercentNonempty", true);

        /// <summary>
        /// Get the current length of the queue.
        /// </summary>
        /// <returns>An integer representing the length.</returns>
        public int GetCurrentLength()
        {
            return this.innerQueue.Count;
        }

        /// <summary>
        /// Dequeue the first entity from the queue.
        /// </summary>
        /// <returns>The dequeued entity.</returns>
        public Entity Dequeue()
        {
            var entity = this.Engine.DequeueEntity<Entity>(this.innerQueue);
            this.Engine.CollectStatistic(this.PercentNonempty, Math.Sign(this.innerQueue.Count));
            this.WriteDebugMessage(entity, "Departed");
            return entity;
        }

        /// <summary>
        /// Return if the initial length is non-zero.
        /// </summary>
        /// <returns>True if initial length is greater than 0. False otherwise.</returns>
        public bool NonZeroInitialLength()
        {
            return this.initialLengthValue > 0;
        }

        /// <inheritdoc/>
        public override void InitializeRun(int runIndex)
        {
            base.InitializeRun(runIndex);
            scanTriggered = false;

            CombiList.ExceptionIfNullOrEmpty(nameof(CombiList));

            if (this.NonZeroInitialLength())
            {
                this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(this.InitializeLength), 0);
            }
        }

        /// <inheritdoc/>
        public override void TransferIn(Entity entity)
        {
            this.WriteDebugMessage(entity, "Arrived");
            this.Engine.EnqueueEntity(entity, this.innerQueue);
            this.Engine.CollectStatistic(this.PercentNonempty, Math.Sign(this.innerQueue.Count));

            if (!scanTriggered)
            {
                this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(Scan), 0);
                scanTriggered = true;
            }
        }

        private static void Scan(Entity entity)
        {
            foreach (var combi in CombiList)
            {
                combi.ExceptionIfNull(nameof(combi));

                while (combi.TryExecute())
                {
                    // do nothing
                    continue;
                }
            }

            scanTriggered = false;
        }

        private void InitializeLength(Entity entity)
        {
            this.WriteDebugMessage(entity, string.Format("Initialized to length {0}", this.initialLengthValue));

            for (int i = 0; i < this.initialLengthValue; i++)
            {
                this.Engine.EnqueueEntity(new Entity(), this.innerQueue);
            }

            this.Engine.CollectStatistic(this.PercentNonempty, Math.Sign(this.innerQueue.Count));

            if (!scanTriggered)
            {
                this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(Scan), 0);
                this.WriteDebugMessage(entity, "Scan Triggered");
                scanTriggered = true;
            }
        }
    }
}
