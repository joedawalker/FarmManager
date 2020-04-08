using System;
using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
    public class MaintenanceSchedule
    {
        [JsonPropertyName( "id" )]
        public int Id { get; set; }

        [JsonPropertyName( "start_date" )]
        public DateTime StartDate { get; set; }

        [JsonPropertyName( "recuring_type" )]
        public RecuringType RecuringType { get; set; }
    }
}
