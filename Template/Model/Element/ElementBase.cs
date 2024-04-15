namespace CYCLONE.Template.Model.Element
{
	using Simphony;
	using Simphony.Simulation;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Text;
	using CYCLONE.Template.Model.Exception;

	public abstract class ElementBase(string label, string? description, NetworkType type): IElement
	{
		private readonly List<WaitingFile> waitingFiles = [];
		private readonly List<Statistic> statistics = [];
		private readonly List<Resource> resources = [];

		private bool isInitialized = false;

		protected DiscreteEventEngine Engine { get; private set; } = new();

		public string? Description { get; } = description;
		public string Label { get; } = label;
		public NetworkType NetworkType { get; } = type;

		public bool Debug { get; set; } = false;

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

			foreach(var resource in this.resources)
			{
				resource.InitializeRun(runIndex);
			}

			this.isInitialized = true;
		}

		public void SetDiscreteEventEngine(DiscreteEventEngine engine)
		{
			this.Engine = engine;
		}

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

		public abstract void TransferIn(Entity entity);

		protected void AddResource(params Resource[] resources)
		{
			resources.ExceptionIfNull(nameof(resources));
			resources.ExceptionIfContainsNull(nameof(resources));
			this.resources.AddRange(resources);

		}

		protected void AddWaitingFile(params WaitingFile[] waitingFiles)
		{
			waitingFiles.ExceptionIfNull(nameof(waitingFiles));
			waitingFiles.ExceptionIfContainsNull(nameof(waitingFiles));
			this.waitingFiles.AddRange(waitingFiles);
		}

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
