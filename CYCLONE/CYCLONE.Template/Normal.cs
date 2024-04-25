namespace CYCLONE.Template
{
    using System.Collections.Generic;
    using System.Text;
    using CYCLONE.Template.Interfaces;
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Model.Exception;
    using CYCLONE.Template.Parameters;
    using CYCLONE.Template.Types;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a Normal element in the CYCLONE model.
    /// </summary>
    public class Normal : CycloneElementBase, IAddFollowers<CycloneNetworkType>
    {
        private readonly NonStaionaryParameters? nstParameters;
        private Distribution duration;

        private double lastTime;
        private bool firstEntity = false;
        private int entityCount = 0;
        private bool firstIncrement = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Normal"/> class.
        /// </summary>
        /// <param name="label">The label of the element.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="duration">The duration distribution.</param>
        /// <param name="nstParameters">The <see cref="NonStaionaryParameters"/> for Non stationary inputs.</param>
        public Normal(string label, string description, Distribution duration, NonStaionaryParameters? nstParameters = null)
            : base(label, description, CycloneNetworkType.NORMAL)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.duration = duration;
            this.nstParameters = nstParameters;

            Distribution.Seed(nstParameters?.Seed ?? 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Normal"/> class.
        /// </summary>
        /// <param name="label">The label of the element.</param>
        /// <param name="description">The description of the element.</param>
        /// <param name="duration">The duration distribution.</param>
        /// <param name="inheritType">The <see cref="CycloneNetworkType"/> of the inheriting element.</param>
        protected Normal(string label, string description, Distribution duration, CycloneNetworkType inheritType, NonStaionaryParameters? nstParameters = null)
            : base(label, description, inheritType)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.duration = duration;
            this.nstParameters = nstParameters;

            Distribution.Seed(nstParameters?.Seed ?? 0);
        }

        /// <summary>
        /// Gets the InterArrivalTime statistic.
        /// </summary>
        public NumericStatistic InterArrivalTime { get; } = new("InterArrivalTime", false);
        
        /// <summary>
        /// Gets the followers of the element.
        /// </summary>
        protected IList<IElement<CycloneNetworkType>> Followers { get; } = [];

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
            this.entityCount++;

            if (this.entityCount > this.nstParameters?.RealizationNumber && this.firstIncrement)
            {
                var increment = this.nstParameters.Increment;
                var seed = this.nstParameters.Seed;

                this.duration = IncrementDistributionValue(this.duration, increment, seed);
                this.WriteDebugMessage(entity, $"Incremented distribution {this.duration.GetType()} by {increment}");
                this.firstIncrement = false;
            }

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

        /// <inheritdoc/>
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

        private static Distribution IncrementDistributionValue(Distribution distribution, double increment, int seed)
        {
            if (distribution is Simphony.Mathematics.Normal normal)
            {
                return new Simphony.Mathematics.Normal(normal.Mean + increment, normal.StandardDeviation);
            }
            else if (distribution is Constant constat)
            {
                return new Constant(constat.Value + increment);
            }
            else if (distribution is Exponential exponential)
            {
                return new Exponential(exponential.Rate + increment);
            }
            else if (distribution is Triangular triangular)
            {
                return new Triangular(triangular.Minimum + increment, triangular.Maximum + increment, triangular.Mode + increment);
            }
            else if (distribution is Uniform uniform)
            {
                return new Uniform(uniform.Minimum + increment, uniform.Maximum + increment);
            }
            else if (distribution is LogNormal logNormal)
            {
                return new LogNormal(logNormal.Scale + increment, logNormal.Shape);
            }
            else if (distribution is Beta beta)
            {
                return new Beta(beta.Shape1, beta.Shape2, beta.Low + increment, beta.High + increment);
            }
            else
            {
                throw new InvalidOperationException("Distribution type not supported");
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

            for (int i = 0; i < this.Followers.Count; i++)
            {
                this.Followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }
    }
}
