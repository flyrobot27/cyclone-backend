namespace CYCLONE.Template.Interfaces
{
    /// <summary>
    /// Interface for allowing an element to add followers.
    /// </summary>
    ///  <typeparam name="T">The Type to distinguish the type of the element.</typeparam>
    public interface IAddFollowers<T>
        where T : Enum
    {
        /// <summary>
        /// Add follower(s) to the list of followers.
        /// </summary>
        /// <param name="elements">The following <see cref="IElement"/>.</param>
        void AddFollowers(params IElement<T>[] elements);
    }
}
