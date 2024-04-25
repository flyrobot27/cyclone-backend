namespace CYCLONE.API.JSONDecode.Blocks.NetworkInput
{
    using CYCLONE.API.JSONDecode;
    using CYCLONE.API.JSONDecode.Blocks;
    using CYCLONE.API.JSONDecode.Blocks.DurationInput;

    /// <summary>
    /// Normal network block class representing the JSON structure in Network Input.
    /// </summary>
    public class NormalNetworkBlock : NetworkBlock, IBlockHasFollowers, IBlockHasSet
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
