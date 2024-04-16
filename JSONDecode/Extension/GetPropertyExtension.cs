namespace CYCLONE.JSONDecode.Extension
{
    using System.Text.Json;

    /// <summary>
    /// Extnsion class for getting property from <see cref="JsonElement"/>.
    /// </summary>
    public static class GetPropertyExtension
    {
        /// <summary>
        /// Get property from <see cref="JsonElement"/> ignoring case.
        /// </summary>
        /// <param name="element">The <see cref="JsonElement"/>.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The relevant Json Sub elements which are also <see cref="JsonElement"/>.</returns>
        /// <exception cref="KeyNotFoundException">Cannot locate property with given <paramref name="propertyName"/>.</exception>
        public static JsonElement GetPropertyIgnoreCase(this JsonElement element, string propertyName)
        {
            var property = element
                .EnumerateObject()
                .FirstOrDefault(p => string.Compare(p.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0);

            if (object.Equals(property, default(JsonProperty)))
            {
                throw new KeyNotFoundException("Cannot find key in json: " + propertyName);
            }

            return property.Value;
        }
    }
}
