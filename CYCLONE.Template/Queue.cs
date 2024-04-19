namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Exception;
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Types;
    using Simphony;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a QUEUE element in the CYCLONE model.
    /// </summary>
    public class Queue : CycloneElementBase
    {
        private static readonly IList<Combi> CombiList = [];
        private static bool scanTriggered = false;

        private readonly int initialLengthValue;
        private readonly WaitingFile innerQueue = new();
        private readonly int multiplyByValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue"/> class.
        /// </summary>
        /// <param name="label">The unique label of the queue.</param>
        /// <param name="description">The description of the queue.</param>
        /// <param name="initialLength">The initial length of the queue.</param>
        /// <param name="multiplyByValue">The values to multiply the number of entities by.</param>
        public Queue(string label, string? description, int initialLength = 0, int multiplyByValue = 1)
            : base(label, description, CycloneNetworkType.QUEUE)
        {
            initialLength.ExceptionIfNegative(nameof(initialLength));
            multiplyByValue.ExceptionIfNotPositive(nameof(multiplyByValue));
            this.AddWaitingFile(this.innerQueue);
            this.AddStatistics(this.PercentNonempty);

            this.multiplyByValue = multiplyByValue;
            this.initialLengthValue = initialLength;
        }

        /// <summary>
        /// Gets the numeric statistics of the queue.
        /// </summary>
        public NumericStatistic PercentNonempty { get; } = new("PercentNonempty", true);

        /// <summary>
        /// Add a combi to the list of combis. Shared for all instances of the <see cref="Queue"/>.
        /// </summary>
        /// <param name="combi">The combi element.</param>
        public static void AddCombi(Combi combi)
        {
            CombiList.Add(combi);
        }

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
                // Multiply the number of entities by the value
                for (var outputCount = 1; outputCount <= this.multiplyByValue; outputCount++)
                {
                    this.Engine.ScheduleEvent(new Entity(), new Action<Entity>(Scan), 0);
                    scanTriggered = true;
                }
            }
        }

        private static void Scan(Entity entity)
        {
            foreach (var combi in CombiList)
            {
                if (combi == null)
                {
                    throw new ModelExecutionException("Null items in " + nameof(CombiList));
                }

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
