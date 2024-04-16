namespace CYCLONE.Template
{
    using System.Collections.Generic;
    using System.Text;
    using CYCLONE.Template.Interfaces;
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Model.Exception;
    using CYCLONE.Template.Types;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a Normal element in the CYCLONE model.
    /// </summary>
    public class Normal : CycloneElementBase, IAddFollowers<CycloneNetworkType>
    {
        private readonly Distribution duration;

        private double lastTime;
        private bool firstEntity = false;

        protected readonly IList<IElement<CycloneNetworkType>> Followers = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="Normal"/> class.
        /// </summary>
        /// <param name="label">The label of the element. Must be unique across all elements.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="duration">The duration distribution.</param>
        public Normal(string label, string description, Distribution duration)
            : base(label, description, CycloneNetworkType.NORMAL)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Normal"/> class.
        /// </summary>
        /// <param name="label">The label of the element. Must be unique across all elements.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="duration">The duration distribution.</param>
        /// <param name="inheritType">The <see cref="CycloneNetworkType"/> of the inheriting element.</param>
        protected Normal(string label, string description, Distribution duration, CycloneNetworkType inheritType)
            : base(label, description, inheritType)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.duration = duration;
        }

        /// <summary>
        /// Gets the InterArrivalTime statistic.
        /// </summary>
        public NumericStatistic InterArrivalTime { get; } = new("InterArrivalTime", false);

        /// <inheritdoc/>
        public override void InitializeRun(int runIndex)
        {
            base.InitializeRun(runIndex);
            this.lastTime = double.NaN;
            this.firstEntity = true;
        }

        /// <inheritdoc/>
        public override void TransferIn(Entity entity)
        {
            this.WriteDebugMessage(entity, "Arrived");
            if (!this.firstEntity)
            {
                this.Engine.CollectStatistic(this.InterArrivalTime, this.Engine.TimeNow - this.lastTime);
            }
            else
            {
                this.firstEntity = false;
            }

            this.lastTime = this.Engine.TimeNow;

            // transfer out to follower
            try
            {
                var handler = new Action<Entity>(this.OnTransferOut);
                this.Engine.ScheduleEvent(entity, handler, this.duration.Sample());
            }
            catch (ModelExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ModelExecutionException(ex, this);
            }
        }

        /// <inheritdoc/>
        public void AddFollowers(params IElement<CycloneNetworkType>[] followers)
        {
            if (followers.GetType() == typeof(Combi))
            {
                throw new InvalidOperationException("Combi cannot be a follower of Normal");
            }

            foreach (var follower in followers)
            {
                this.Followers.Add(follower);
            }
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var sb = new StringBuilder(baseString);
            sb.AppendLine("Followers:");
            foreach (var follower in this.Followers)
            {
                sb.AppendLine($"  {follower.ToString()}");
            }

            return sb.ToString();
        }

        private void OnTransferOut(Entity entity)
        {
            this.WriteDebugMessage(entity, "Departed");
            if (this.Engine.CurrentEntity != entity)
            {
                this.Engine.ScheduleEvent(entity, this.OnTransferOut, 0D);
                return;
            }

            for (int i = 0; i < this.Followers.Count; i++)
            {
                this.Followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }
    }
}
