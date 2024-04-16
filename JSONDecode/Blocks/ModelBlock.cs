namespace CYCLONE.JSONDecode.Blocks
{
    using System.Text.Json.Serialization;
    using CYCLONE.JSONDecode.Blocks.NetworkInput;
    using CYCLONE.Template.Types;

    /// <summary>
    /// Model Block class representing the JSON structure.
    /// </summary>
    public class ModelBlock
    {
        /// <summary>
        /// Gets or sets the type of the main block.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        required public MainType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        required public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the length of the run.
        /// </summary>
        required public int LengthOfRun { get; set; }

        /// <summary>
        /// Gets or sets the number of cycles.
        /// </summary>
        required public int NoOfCycles { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="NetworkBlock"/> in the Network Input.
        /// </summary>
        required public List<NetworkBlock> NetworkInput { get; set; }
    }
}
