using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
    public class MaintenanceItem
    {
        [JsonPropertyName( "id" )]
        public int Id { get; set; }

        [JsonPropertyName( "name" )]
        public string Name { get; set; }

        [JsonPropertyName( "instructions" )]
        public string Instructions { get; set; }

        [JsonPropertyName( "required_parts" )]
        public List<RequiredEquipmentPart> RequiredParts { get; set; }

        [JsonPropertyName( "schedule" )]
        public MaintenanceSchedule Schedule { get; set; }

        [JsonPropertyName( "maintenance_log" )]
        public List<MaintenanceLogRecord> MaintenanceLog { get; set; }

        [JsonPropertyName( "next_service" )]
        public DateTime? NextService { get; set; }
    }
}