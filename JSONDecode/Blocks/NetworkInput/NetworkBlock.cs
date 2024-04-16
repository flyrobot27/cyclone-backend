namespace CYCLONE.JSONDecode.Blocks.NetworkInput
{
    using System.Text.Json.Serialization;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Network block class representing the JSON structure Network Input.
    /// </summary>
    public class NetworkBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="NetworkBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public CycloneNetworkType Type { get; set; }

        /// <summary>
        /// Gets or sets the label of the <see cref="NetworkBlock"/>.
        /// </summary>
        required public string Label { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="NetworkBlock"/>.
        /// </summary>
        required public string Description { get; set; }
    }
}
