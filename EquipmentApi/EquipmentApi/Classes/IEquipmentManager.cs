using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
	public interface IEquipmentManager
	{
		Task<List<EquipmentValidationFailureType>> CreateEquipmentItemAsync( EquipmentItem equipmentItem );
		Task<List<EquipmentValidationFailureType>> DeleteEquipmentItemAsync( int id );
		Task<Tuple<EquipmentItem, List<EquipmentValidationFailureType>>> GetEquipmentItemAsync( int id );
		Task<List<EquipmentItem>> GetEquipmentItemsAsync();

		/// <summary>
		/// Calculates the next scheduled date for a maintenance item.
		/// </summary>
		/// <param name="maintenanceItem"></param>
		DateTime? GetNextService( MaintenanceItem maintenanceItem );
		Task<List<EquipmentValidationFailureType>> UpdateEquipmentItemAsync( EquipmentItem equipmentItem );
	}
}