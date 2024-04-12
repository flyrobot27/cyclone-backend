namespace CYCLONE.Template.Model.Element
{
	using Simphony;
	using Simphony.Simulation;
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Text;

	public abstract class ElementBase(string id, string? description)
	{
		protected DiscreteEventEngine Engine = new();
		private readonly List<WaitingFile> waitingFiles = new List<WaitingFile>();
		private readonly List<Statistic> statistics = new List<Statistic>();
		private readonly List<Resource> resources = new List<Resource>();

		public string? Description { get; } = description;
		public string ID { get; } = id;

		public bool Debug { get; set; } = false;

		public virtual void FinalizeRun(int runIndex)
		{
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

		protected void AddResource(params Resource[] resources)
		{
			resources.ExceptionIfNull(nameof(resources));
			resources.ExceptionIfContainsNull("resources");
			this.resources.AddRange(resources);

		}

		protected void AddWaitingFile(params WaitingFile[] waitingFiles)
		{
			waitingFiles.ExceptionIfNull(nameof(waitingFiles));
			waitingFiles.ExceptionIfContainsNull("waitingFiles");
			this.waitingFiles.AddRange(waitingFiles);
		}

		protected void AddStatistics(params Statistic[] statistics)
		{
			statistics.ExceptionIfNull(nameof(statistics));
			statistics.ExceptionIfContainsNull("statistics");
			this.statistics.AddRange(statistics);
		}

		protected void ClearWaitingFiles() 
		{ 
			this.waitingFiles.Clear(); 
		}
	}
}
