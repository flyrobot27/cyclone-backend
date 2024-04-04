namespace CYCLONE.Template.Model.Element
{
	using Simphony;
	using Simphony.Simulation;
	using System;
	public abstract class ElementBase(string id, string? description)
	{
		protected DiscreteEventEngine Engine = new();
		private readonly List<WaitingFile> waitingFiles = new List<WaitingFile>();
		private readonly List<Statistic> statistics = new List<Statistic>();
		private readonly List<Resource> resources = new List<Resource>();

		public string? Description { get; } = description;
		public string ID { get; } = id;

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
