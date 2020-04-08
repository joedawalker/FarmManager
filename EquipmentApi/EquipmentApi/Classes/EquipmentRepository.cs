using EquipmentApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
	public class EquipmentRepository : IEquipmentRepository
	{
		private readonly EquipmentApiContext _equipmentContext;

		public EquipmentRepository( EquipmentApiContext context )
		{
			_equipmentContext = context;
		}

		#region EquipmentItems
		public async Task<List<EquipmentItem>> GetEquipmentAsync()
		{
			List<EquipmentItem> equipment = await _equipmentContext.EquipmentItems
				.Include( q => q.MaintenanceItems ).ThenInclude( m => m.Schedule )
				.Include( q => q.MaintenanceItems ).ThenInclude( m => m.MaintenanceLog ).ThenInclude( ml => ml.UpdatedBy )
				.Include( q => q.MaintenanceItems ).ThenInclude( m => m.RequiredParts ).ThenInclude( r => r.Part ).ToListAsync().ConfigureAwait( false );

			return equipment;
		}

		public async Task<EquipmentItem> GetEquipmentItemAsync( int id )
		{
			EquipmentItem equipment = await _equipmentContext.EquipmentItems
				.Include( e => e.MaintenanceItems ).ThenInclude( m => m.Schedule )
				.Include( e => e.MaintenanceItems ).ThenInclude( m => m.MaintenanceLog ).ThenInclude( ml => ml.UpdatedBy )
				.Include( e => e.MaintenanceItems ).ThenInclude( m => m.RequiredParts ).ThenInclude( r => r.Part )
				.FirstOrDefaultAsync( e => e.Id == id ).ConfigureAwait( false );

			return equipment;
		}

		public async Task<bool> CreateEquipmentItemAsync( EquipmentItem equipmentItem )
		{
			try
			{
				foreach ( var item in equipmentItem.MaintenanceItems )
				{
					if ( item == null )
					{
						break;
					}

					Task<List<int>> partsTask = _equipmentContext.EquipmentParts.Select( p => p.Id ).ToListAsync();
					_equipmentContext.MaintenanceLog.AddRange( item.MaintenanceLog );
					_equipmentContext.MaintenanceSchedules.Add( item.Schedule );

					await partsTask.ConfigureAwait( false );

					_equipmentContext.EquipmentParts.AddRange( item.RequiredParts.Select( p => p.Part ).Where( p => !partsTask.Result.Contains( p.Id ) ).ToList() );
					_equipmentContext.RequiredEquipmentParts.AddRange( item.RequiredParts );
					_equipmentContext.MaintenanceItems.Add( item );
				}

				_equipmentContext.EquipmentItems.Add( equipmentItem );

				await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );

				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> UpdateEquipmentItemAsync( EquipmentItem equipmentItem )
		{
			try
			{
				EquipmentItem existingItem = await GetEquipmentItemAsync( equipmentItem.Id ).ConfigureAwait( false );

				existingItem.Name = equipmentItem.Name;
				existingItem.HoursUsed = equipmentItem.HoursUsed;
				existingItem.Mileage = equipmentItem.Mileage;
				existingItem.Notes = equipmentItem.Notes;

				UpdateMaintenanceItems( equipmentItem, equipmentItem );

				_equipmentContext.EquipmentItems.Update( existingItem );
				await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );

				return true;
			}
			catch
			{
				return false;
			}
		}

		private void UpdateMaintenanceItems( EquipmentItem existingEquipment, EquipmentItem newEquipment )
		{
			List<MaintenanceItem> itemsToRemove = new List<MaintenanceItem>();

			foreach ( var maintenanceItem in existingEquipment.MaintenanceItems )
			{
				MaintenanceItem newItem = newEquipment.MaintenanceItems.FirstOrDefault( m => m.Id == maintenanceItem.Id );
				if ( newItem == null )
				{
					itemsToRemove.Add( maintenanceItem );
				}
				else
				{
					maintenanceItem.Name = newItem.Name;
					maintenanceItem.Instructions = newItem.Instructions;
					maintenanceItem.NextService = newItem.NextService;

					if ( maintenanceItem.Schedule != newItem.Schedule )
					{
						_equipmentContext.MaintenanceSchedules.Remove( maintenanceItem.Schedule );
						maintenanceItem.Schedule.RecuringType = newItem.Schedule.RecuringType;
						maintenanceItem.Schedule.StartDate = newItem.Schedule.StartDate;
						maintenanceItem.Schedule.Id = newItem.Id;
					}

					// TODO: update required parts and maintenance log
				}
			}
		}

		public async Task<bool> DeleteEquipmentItemAsync( int id )
		{
			EquipmentItem equipment = await _equipmentContext.EquipmentItems
				.Include( e => e.MaintenanceItems ).ThenInclude( m => m.Schedule )
				.Include( e => e.MaintenanceItems ).ThenInclude( m => m.MaintenanceLog )
				.Include( e => e.MaintenanceItems ).ThenInclude( m => m.RequiredParts ).ThenInclude( r => r.Part )
				.FirstOrDefaultAsync( e => e.Id == id ).ConfigureAwait( false );

			foreach ( var maintenanceItem in equipment.MaintenanceItems )
			{
				DeleteMaintenanceItemWithoutSavingAsync( maintenanceItem );
			}

			_equipmentContext.MaintenanceItems.RemoveRange( equipment.MaintenanceItems );

			await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );

			return true;
		}
		#endregion

		#region MaintenanceItems
		public async Task<bool> DeleteMaintenanceItemAsync( int id )
		{
			DeleteMaintenanceItemWithoutSavingAsync( await _equipmentContext.MaintenanceItems
				.Include( m => m.MaintenanceLog )
				.Include( m => m.RequiredParts ).ThenInclude( r => r.Part )
				.Include( m => m.Schedule )
				.FirstOrDefaultAsync( m => m.Id == id ).ConfigureAwait( false ) );

			await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );

			return true;
		}

		private void DeleteMaintenanceItemWithoutSavingAsync( MaintenanceItem maintenanceItem )
		{
			_equipmentContext.MaintenanceLog.RemoveRange( maintenanceItem.MaintenanceLog );

			_equipmentContext.MaintenanceSchedules.Remove( maintenanceItem.Schedule );

			foreach ( var requiredPart in maintenanceItem.RequiredParts )
			{
				_equipmentContext.EquipmentParts.Remove( requiredPart.Part );
			}

			_equipmentContext.RequiredEquipmentParts.RemoveRange( maintenanceItem.RequiredParts );
		}
		#endregion
	}
}
