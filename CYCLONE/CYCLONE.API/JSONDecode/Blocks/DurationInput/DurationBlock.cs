﻿namespace CYCLONE.API.JSONDecode.Blocks.DurationInput
{
    using System.Text.Json.Serialization;
    using CYCLONE.API.JSONDecode.Blocks.DistrbutionBlock;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Duration block class representing the JSON structure for Duration Input.
    /// </summary>
    public class DurationBlock
    {
        /// <summary>
        /// Gets or sets the type of the <see cref="DurationBlock"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public DurationType Type { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DistributionBlock"/> of the <see cref="DurationBlock"/>.
        /// </summary>
        required public DistributionBlock Distribution { get; set; }
    }
}
