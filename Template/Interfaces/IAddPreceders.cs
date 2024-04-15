namespace CYCLONE.Template.Interfaces
{
    /// <summary>
    /// Interface for allowing an element to add preceders.
    /// </summary>
    ///  <typeparam name="T">The Type to distinguish the type of the element.</typeparam>
    public interface IAddPreceders<T>
        where T : Enum
    {
        /// <summary>
        /// Add preceder(s) to the list of preceders.
        /// </summary>
        /// <param name="elements">The preceding element(s).</param>
        void AddPreceders(IElement<T>[] elements);
    }
}
