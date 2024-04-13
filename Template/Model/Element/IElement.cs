namespace CYCLONE.Template.Model.Element
{
    using Simphony.Simulation;

    public interface IElement
    {
        void TransferIn(Entity entity);
        void InitializeRun(int runIndex);
        void FinalizeRun(int runIndex);
        void SetDiscreteEventEngine(DiscreteEventEngine engine);
        NetworkType NetworkType { get; }
    }
}
