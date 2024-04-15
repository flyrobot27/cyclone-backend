namespace CYCLONE.JSONDecode.Extension
{
    using System.Text.Json;

    public static class GetPropertyExtension
    {
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
