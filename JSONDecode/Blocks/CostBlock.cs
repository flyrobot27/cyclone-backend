namespace CYCLONE.API.JSONDecode.Blocks
{
    using System.Text.Json.Serialization;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Cost block class representing the JSON structure for Resource Cost.
    /// </summary>
    public class CostBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="CostBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public ResourceCostType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="CostBlock"/>.
        /// </summary>
        required public double Value { get; set; }
    }
}
