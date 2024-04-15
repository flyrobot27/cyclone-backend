namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;
    using Simphony;
    using Simphony.Simulation;
    using System.Collections.Generic;

    public class Consolidate(string label, string? description, IList<IElement> followers, int DivideByValue = 1, int MultiplyByValue = 1) : 
		ElementFunction(label, description, followers, NetworkType.FUNCTION_CONSOLIDATE)
	{
		private int InputCount = 0;
		private readonly int DivideByValue = DivideByValue;
		private readonly int MultiplyByValue = MultiplyByValue;

		public override void TransferIn(Entity entity)
		{
			entity.ExceptionIfNull(nameof(entity));

			this.WriteDebugMessage(entity, "Arrived at Consolidate");
			this.InputCount++;

			if (this.InputCount == this.DivideByValue)
			{
				for (int outputCount = 1; outputCount <= this.MultiplyByValue; outputCount++)
				{
					this.WriteDebugMessage(entity, "Departed Consolidate");
					if (outputCount == 1)
					{
						base.TransferIn(entity);
					}
					else
					{
						base.TransferIn(entity.Clone());
					}
				}
			}
		}
	}
}
