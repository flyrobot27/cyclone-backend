namespace CYCLONE.JSONDecode.Blocks.DistrbutionBlock
{
    using System.Text.Json.Serialization;
    using CYCLONE.Types;

    /// <summary>
    /// Distribution block class representing the JSON structure for a Statistic Distribution.
    /// </summary>
    public class DistributionBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="DistributionBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public DistributionType Type { get; set; }
    }
}
