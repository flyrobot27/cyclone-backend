namespace CYCLONE.Template.Model.Element
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text;

    using CYCLONE.Template.Model.Exception;
    using CYCLONE.Types;
    using Simphony;
    using Simphony.Simulation;

    /// <summary>
    /// Base class for all elements in the model.
    /// </summary>
    public abstract class ElementBase
        : IElement
    {
        private static readonly List<string> ExistingLabels = [];

        private readonly List<WaitingFile> waitingFiles = [];
        private readonly List<Statistic> statistics = [];
        private readonly List<Resource> resources = [];

        private bool isInitialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementBase"/> class.
        /// </summary>
        /// <param name="label">The label of the element. Must be unique across all elements.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="type">The Network Type of the element.</param>
        public ElementBase(string label, string? description, NetworkType type)
        {
            this.Description = description;
            
            if (ExistingLabels.Contains(label))
            {
                throw new InvalidOperationException($"Label {label} already exists");
            }

            ExistingLabels.Add(label);
            this.Label = label;
            this.NetworkType = type;
        }

        /// <summary>
        /// Gets the description of the element.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Gets the label of the element.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the Network Type of the element.
        /// </summary>
        public NetworkType NetworkType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether debugging is enabled.
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets the Discrete Event Engine.
        /// </summary>
        protected DiscreteEventEngine Engine { get; private set; } = new ();

        /// <summary>
        /// Finalizes the element for a simulation run.
        /// </summary>
        /// <param name="runIndex">The run index. Useful when using Monte-Carlo Simulation. If only a single run, set runIndex to 0.</param>
        public virtual void FinalizeRun(int runIndex)
        {
            this.ExceptionIfNotInitialized();

            foreach (var wf in this.waitingFiles)
            {
                wf.FinalizeRun(runIndex, this.Engine.TimeNow);
            }

            foreach (var stats in this.statistics)
            {
                stats.FinalizeRun(runIndex, this.Engine.TimeNow);
            }

            foreach (var resource in this.resources)
            {
                resource.FinalizeRun(runIndex, this.Engine.TimeNow);
            }
        }

        /// <summary>
        /// Initialize the element for a simulation run.
        /// </summary>
        /// <param name="runIndex">The run index. Useful when using Monte-Carlo Simulation. If only a single run, set runIndex to 0.</param>
        public virtual void InitializeRun(int runIndex)
        {
            foreach (var wf in this.waitingFiles)
            {
                wf.InitializeRun(runIndex);
            }

            foreach (var stats in this.statistics)
            {
                stats.InitializeRun(runIndex);
            }

            foreach (var resource in this.resources)
            {
                resource.InitializeRun(runIndex);
            }

            this.isInitialized = true;
        }

        /// <summary>
        /// Sets the Discrete Event Engine.
        /// </summary>
        /// <param name="engine">The Discrete Event Engine.</param>
        public void SetDiscreteEventEngine(DiscreteEventEngine engine)
        {
            this.Engine = engine;
        }

        /// <summary>
        /// Writes a debug message to the trace.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message to print.</param>
        public void WriteDebugMessage(Entity entity, string? message)
        {
            if (this.Debug)
            {
                var converter = new TypeConverter();
                var builder = new StringBuilder();

                builder.Append("Debugging Output:");
                builder.Append('\t');
                builder.Append(this.Engine.RunIndex + 1);
                builder.Append('\t');
                builder.Append(converter.ConvertToString(this.Engine.TimeNow));
                builder.Append('\t');
                builder.Append(this.GetType().Name);
                builder.Append('\t');
                builder.Append(this.Description);
                builder.Append('\t');

                string entityType = entity.GetType().Name;
                builder.Append(entityType);
                builder.Append('\t');

                string entityName = entity.Name;
                builder.Append(entityName);
                builder.Append('\t');

                if (message != null)
                {
                    builder.Append(message);
                }

                Trace.WriteLine(builder, "Debug");
            }
        }

        /// <summary>
        /// Abstract method that defines the behavior when an entity is transferred in.
        /// </summary>
        /// <param name="entity">The entity to be transferred in.</param>
        public abstract void TransferIn(Entity entity);

        /// <summary>
        /// Adds resource(s) to the element.
        /// </summary>
        /// <param name="resources">The resource(s).</param>
        protected void AddResource(params Resource[] resources)
        {
            resources.ExceptionIfNull(nameof(resources));
            resources.ExceptionIfContainsNull(nameof(resources));
            this.resources.AddRange(resources);
        }

        /// <summary>
        /// Adds waiting file(s) to the element.
        /// </summary>
        /// <param name="waitingFiles">The Waiting File(s).</param>
        protected void AddWaitingFile(params WaitingFile[] waitingFiles)
        {
            waitingFiles.ExceptionIfNull(nameof(waitingFiles));
            waitingFiles.ExceptionIfContainsNull(nameof(waitingFiles));
            this.waitingFiles.AddRange(waitingFiles);
        }

        /// <summary>
        /// Adds statistic(s) to the element.
        /// </summary>
        /// <param name="statistics">The Statistic(s).</param>
        protected void AddStatistics(params Statistic[] statistics)
        {
            statistics.ExceptionIfNull(nameof(statistics));
            statistics.ExceptionIfContainsNull(nameof(statistics));
            this.statistics.AddRange(statistics);
        }

        private void ExceptionIfNotInitialized()
        {
            if (!this.isInitialized)
            {
                throw new ModelExecutionException("Element is not initialized");
            }
        }
    }
}
