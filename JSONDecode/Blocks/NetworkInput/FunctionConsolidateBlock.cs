namespace CYCLONE.JSONDecode.Blocks.NetworkInput
{
    using CYCLONE.JSONDecode;
    using CYCLONE.JSONDecode.Blocks;

    /// <summary>
    /// Function Consolidate block class representing the JSON structure in Network Input.
    /// </summary>
    public class FunctionConsolidateBlock : NetworkBlock, IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the Number of entities to consolidate.
        /// </summary>
        required public int NumberToConsolidate { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> following the <see cref="FunctionConsolidateBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }
    }
}
