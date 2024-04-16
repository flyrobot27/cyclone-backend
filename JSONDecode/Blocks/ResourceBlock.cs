namespace CYCLONE.JSONDecode.Blocks
{
    using System.Text.Json.Serialization;
    using CYCLONE.Types;

    /// <summary>
    /// Resource block class representing the JSON structure for Resource Input.
    /// </summary>
    public class ResourceBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="ResourceBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ResourceType Type { get; set; }

        /// <summary>
        /// Gets or sets the number of units of the <see cref="ResourceBlock"/>.
        /// </summary>
        required public int NoOfUnit { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="ResourceBlock"/>.
        /// </summary>
        required public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="CostBlock"/> of the <see cref="ResourceBlock"/>.
        /// </summary>
        public List<CostBlock>? Cost { get; set; }
    }
}
