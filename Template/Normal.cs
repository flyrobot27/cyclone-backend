﻿namespace CYCLONE.Template
{
    using System.Collections.Generic;

    using CYCLONE.Template.Model.Element;
    using CYCLONE.Template.Model.Exception;
    using CYCLONE.Types;
    using Simphony.Mathematics;
    using Simphony.Simulation;

    public class Normal : ElementBase, IElement
    {
        protected double lastTime;
        protected bool firstEntity = false;
        protected readonly Distribution Duration;
        protected readonly IList<IElement> Followers;

        public NumericStatistic InterArrivalTime = new("InterArrivalTime", false);

        public Normal(string label, string description, Distribution duration, IList<IElement> followers)
            : base(label, description, NetworkType.NORMAL)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.Duration = duration;
            this.Followers = followers;
        }

        protected Normal(string label, string description, Distribution duration, IList<IElement> followers, NetworkType inheritType)
            : base(label, description, inheritType)
        {
            this.AddStatistics(this.InterArrivalTime);
            this.Duration = duration;
            this.Followers = followers;
        }

        public override void InitializeRun(int runIndex)
        {
            base.InitializeRun(runIndex);
            this.lastTime = double.NaN;
            this.firstEntity = true;
        }

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
                this.Engine.ScheduleEvent(entity, handler, this.Duration.Sample());
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

            for (int i = 0; i < this.Followers.Count; i++)
            {
                this.Followers[i].TransferIn(i == 0 ? entity : entity.Clone());
            }
        }
    }
}
