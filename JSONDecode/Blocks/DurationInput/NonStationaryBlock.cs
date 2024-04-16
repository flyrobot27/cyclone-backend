namespace CYCLONE.JSONDecode.Blocks.DurationInput
{
    using System.Text.Json.Serialization;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Non Stationary block class representing the JSON structure for Duration Input.
    /// </summary>
    public class NonStationaryBlock : DurationBlock
    {
        /// <summary>
        /// Gets or Sets the Non Staionary Category of the <see cref="NonStationaryBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public NSTCategory Category { get; set; }

        /// <summary>
        /// Gets or Sets the Par1 of the <see cref="NonStationaryBlock"/>.
        /// </summary>
        required public double Par1 { get; set; }

        /// <summary>
        /// Gets or Sets the Par2 of the <see cref="NonStationaryBlock"/>. Only used if <see cref="Category"/> is <see cref="NSTCategory.SECOND"/>.
        /// </summary>
        public double? Par2 { get; set; }

        /// <summary>
        /// Gets or Sets the Seed of the <see cref="NonStationaryBlock"/>. Only used if <see cref="Category"/> is <see cref="NSTCategory.SECOND"/>.
        /// </summary>
        public int? Seed { get; set; }
    }
}
