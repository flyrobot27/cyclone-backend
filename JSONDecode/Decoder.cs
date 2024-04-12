namespace CYCLONE.JSONDecode
{
	using System.Text.Json;

	public class Decoder
	{
		public Decoder(string JSONBody)
		{
			var result = JsonSerializer.Deserialize<JsonElement>(JSONBody);
		}
	}
}
