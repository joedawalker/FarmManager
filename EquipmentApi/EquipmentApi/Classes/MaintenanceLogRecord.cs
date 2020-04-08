using System;
using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
    public class MaintenanceLogRecord
    {
        [JsonPropertyName( "id" )]
        public int Id { get; set; }

        [JsonPropertyName( "date" )]
        public DateTime Date { get; set; }

        [JsonPropertyName( "log_type" )]
        public MaintenanceLogType LogType { get; set; }

        [JsonPropertyName( "note" )]
        public string Note { get; set; }

        [JsonPropertyName( "updated_by" )]
        public User UpdatedBy { get; set; }

        [JsonPropertyName( "hours_used_snapshot" )]
        public TimeSpan HoursUsedSnapshot { get; set; }

        [JsonPropertyName( "mileage_snapshot" )]
        public double MileageSnapshot { get; set; }
    }
}
