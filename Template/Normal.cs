namespace CYCLONE.Template
{
    using System.Collections.Generic;

    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Model.Exception;
    using CYCLONE.Types;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a Normal element in the CYCLONE model.
    /// </summary>
    public class Normal : ElementBase, IElement
    {
        private readonly Distribution duration;
        private readonly IList<IElement> followers;

        private double lastTime;
        private bool firstEntity = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Normal"/> class.
        /// </summary>
        /// <param name="label">The label of the element. Must be unique across all elements.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="duration">The duration distribution.</param>
        /// <param name="followers">The followers of the element.</param>
        public Normal(string label, string description, Distribution duration, IList<IElement> followers)
            : base(label, description, NetworkType.NORMAL)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.duration = duration;
            this.followers = followers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Normal"/> class.
        /// </summary>
        /// <param name="label">The label of the element. Must be unique across all elements.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="duration">The duration distribution.</param>
        /// <param name="followers">The followers of the element.</param>
        /// <param name="inheritType">The <see cref="NetworkType"/> of the inheriting element.</param>
        protected Normal(string label, string description, Distribution duration, IList<IElement> followers, NetworkType inheritType)
            : base(label, description, inheritType)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.duration = duration;
            this.followers = followers;
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

        private void OnTransferOut(Entity entity)
        {
            this.WriteDebugMessage(entity, "Departed");
            if (this.Engine.CurrentEntity != entity)
            {
                this.Engine.ScheduleEvent(entity, this.OnTransferOut, 0D);
                return;
            }

            for (int i = 0; i < this.followers.Count; i++)
            {
                this.followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }
    }
}
