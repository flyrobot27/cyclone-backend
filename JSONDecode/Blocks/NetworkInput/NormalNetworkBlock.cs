namespace CYCLONE.JSONDecode.Blocks.NetworkInput
{
    using CYCLONE.JSONDecode;
    using CYCLONE.JSONDecode.Blocks;
    using CYCLONE.JSONDecode.Blocks.DurationInput;

    /// <summary>
    /// Normal network block class representing the JSON structure in Network Input.
    /// </summary>
    public class NormalNetworkBlock : NetworkBlock, IBlockHasFollowers
    {
        /// <summary>
        /// Gets or sets the <see cref="DurationBlock"/> of the <see cref="NormalNetworkBlock"/>.
        /// </summary>
        required public DurationBlock Set { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> follwing the <see cref="NormalNetworkBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }
    }
}
