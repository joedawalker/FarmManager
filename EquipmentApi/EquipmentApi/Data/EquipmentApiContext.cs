using EquipmentApi.Classes;
using Microsoft.EntityFrameworkCore;

namespace EquipmentApi.Data
{
	public class EquipmentApiContext : DbContext
	{
		public EquipmentApiContext( DbContextOptions<EquipmentApiContext> options )
			: base( options ) { }

		public DbSet<User> Users { get; set; }
		public DbSet<EquipmentItem> EquipmentItems { get; set; }
		public DbSet<MaintenanceItem> MaintenanceItems { get; set; }
		public DbSet<RequiredEquipmentPart> RequiredEquipmentParts { get; set; }
		public DbSet<EquipmentPart> EquipmentParts { get; set; }
		public DbSet<MaintenanceLogRecord> MaintenanceLog { get; set; }
		public DbSet<MaintenanceSchedule> MaintenanceSchedules { get; set; }
	}
}
