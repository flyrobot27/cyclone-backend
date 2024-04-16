namespace CYCLONE.API.JSONDecode
{
    using CYCLONE.API.JSONDecode.Blocks;

    /// <summary>
    /// Interface for blocks that have followers.
    /// </summary>
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
