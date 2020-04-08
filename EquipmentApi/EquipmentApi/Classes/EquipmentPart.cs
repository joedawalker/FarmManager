using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
	public class EquipmentPart
	{
		[JsonPropertyName( "id" )]
		public int Id { get; set; }

		[JsonPropertyName( "name" )]
		public string Name { get; set; }

		[JsonPropertyName( "serial_number" )]
		public string SerialNumber { get; set; }
	}
}
