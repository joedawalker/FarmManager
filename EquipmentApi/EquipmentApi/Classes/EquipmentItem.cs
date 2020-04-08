using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
	public class EquipmentItem
	{
		[JsonPropertyName( "id" )]
		public int Id { get; set; }

		[JsonPropertyName( "name" )]
		public string Name { get; set; }

		[JsonPropertyName( "hours_used" )]
		public TimeSpan HoursUsed { get; set; }

		[JsonPropertyName( "mileage" )]
		public double Mileage { get; set; }

		[JsonPropertyName( "maintenance_items" )]
		public List<MaintenanceItem> MaintenanceItems { get; set; }

		[JsonPropertyName( "notes" )]
		public string Notes { get; set; }
	}
}
