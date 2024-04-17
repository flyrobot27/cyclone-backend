namespace CYCLONE.API.JSONDecode.Blocks.NetworkInput
{
    using CYCLONE.API.JSONDecode.Blocks;

    /// <summary>
    /// Queue block class representing the JSON structure in Network Input.
    /// </summary>
    public class QueueBlock : NetworkBlock
    {
        /// <summary>
        /// Gets or sets the Number of entities to be generated.
        /// </summary>
        required public int NumberToBeGenerated { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ResourceBlock"/> of the <see cref="QueueBlock"/>.
        /// </summary>
        public ResourceBlock? ResourceInput { get; set; }
    }
}
