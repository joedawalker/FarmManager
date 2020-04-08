using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
    public class RequiredEquipmentPart
    {
        [JsonPropertyName( "id" )]
        public int Id { get; set; }

        [JsonPropertyName( "part" )]
        public EquipmentPart Part { get; set; }

        [JsonPropertyName( "quantity" )]
        public int Quantity { get; set; }
    }
}
