namespace CYCLONE.API.JSONDecode.Blocks.NetworkInput
{
    using CYCLONE.API.JSONDecode;
    using CYCLONE.API.JSONDecode.Blocks;

    /// <summary>
    /// Function Counter block class representing the JSON structure in Network Input.
    /// </summary>
    public class FunctionCounterBlock : NetworkBlock, IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the quantity of elements initially in the counter.
        /// </summary>
        required public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> following the <see cref="FunctionCounterBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }
    }
}
