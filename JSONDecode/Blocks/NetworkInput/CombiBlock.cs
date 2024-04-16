﻿namespace CYCLONE.JSONDecode.Blocks.NetworkInput
{
    using CYCLONE.JSONDecode;
    using CYCLONE.JSONDecode.Blocks;
    using CYCLONE.JSONDecode.Blocks.DurationInput;

    /// <summary>
    /// Combi block class representing the JSON structure in Network Input.
    /// </summary>
    public class CombiBlock : NetworkBlock, IBlockHasFollowers, IBlockHasPreceders
    {
        /// <summary>
        /// Gets or sets the <see cref="DurationBlock"/> of the <see cref="CombiBlock"/>.
        /// </summary>
        required public DurationBlock Set { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> follwing the <see cref="CombiBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Followers { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ReferenceBlock"/> preceding the <see cref="CombiBlock"/>.
        /// </summary>
        required public List<ReferenceBlock> Preceders { get; set; }
    }
}