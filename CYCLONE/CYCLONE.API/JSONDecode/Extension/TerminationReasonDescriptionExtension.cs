namespace CYCLONE.API.JSONDecode.Extension
{
    using System.ComponentModel;
    using Simphony.Simulation;

    /// <summary>
    /// Extension for extracting the description attribute from <see cref="TerminationReason"/>.
    /// </summary>
    public static class TerminationReasonDescriptionExtension
    {
        /// <summary>
        /// Get the description attribute of <see cref="TerminationReason"/>.
        /// </summary>
        /// <param name="terminationReason"><see cref="TerminationReason">.</param>
        /// <returns>The Description of the termination reason.</returns>
        public static string GetDescriptionAttribute (this TerminationReason terminationReason)
        {
            var type = terminationReason.GetType();
            var memberInfo = type.GetMember(terminationReason.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length == 0)
            {
                return terminationReason.ToString();
            }

            var descriptionAttribute = (DescriptionAttribute)attributes[0];
            return descriptionAttribute.Description;
        }
    }
}
