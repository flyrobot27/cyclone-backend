namespace CYCLONE.Template
{
    using CYCLONE.Template.Model.Element;
    using CYCLONE.Types;
    using Simphony;
    using Simphony.Simulation;

    /// <summary>
    /// Represents a Consolidate element in the CYCLONE model.
    /// </summary>
    /// <param name="label">The label of the element. Must be unique across all elements.</param>
    /// <param name="description">The description of the element.</param>
    /// <param name="divideByValue">The values to divide the number of entities by.</param>
    /// <param name="multiplyByValue">The values to multiply the number of entities by.</param>
    public class Consolidate(string label, string? description, int divideByValue = 1, int multiplyByValue = 1) 
        : ElementFunction(label, description, CycloneNetworkType.FUNCTION_CONSOLIDATE)
    {
        private readonly int divideByValue = divideByValue;
        private readonly int multiplyByValue = multiplyByValue;

        private int inputCount = 0;

        /// <inheritdoc/>
        public override void TransferIn(Entity entity)
        {
            entity.ExceptionIfNull(nameof(entity));

            this.WriteDebugMessage(entity, "Arrived at Consolidate");
            this.inputCount++;

            if (this.inputCount == this.divideByValue)
            {
                for (int outputCount = 1; outputCount <= this.multiplyByValue; outputCount++)
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
