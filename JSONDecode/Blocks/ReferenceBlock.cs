namespace CYCLONE.JSONDecode.Blocks
{
    using System.Text.Json.Serialization;
    using CYCLONE.JSONDecode.Blocks.NetworkInput;
    using CYCLONE.Types;

    /// <summary>
    /// Reference block class representing the JSON structure that is used to reference another <see cref="NetworkBlock"/>.
    /// </summary>
    public class ReferenceBlock
    {
        /// <summary>
        /// Gets or sets the type of the reference block.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ReferenceType Type { get; set; }

        /// <summary>
        /// Gets or sets the label of the <see cref="NetworkBlock"/> the reference block is referring to.
        /// </summary>
        required public string Value { get; set; }
    }
}
