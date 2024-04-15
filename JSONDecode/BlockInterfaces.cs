namespace CYCLONE.JSONDecode
{
    /// <summary>
    /// Interface for blocks that have followers.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Too many cluttered files otherwise.")]
    public interface IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the followers of the block.
        /// </summary>
        List<ReferenceBlock> Followers { get; set; }
    }

    /// <summary>
    /// Interface for blocks that have preceders.
    /// </summary>
    public interface IBlockHasPreceders
    {
        /// <summary>
        /// Gets or sets the preceders of the block.
        /// </summary>
        List<ReferenceBlock> Preceders { get; set; }
    }

}
