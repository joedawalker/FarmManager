using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
	public class EquipmentManager : IEquipmentManager
	{
		private readonly IEquipmentRepository _equipmentRepository;

		public EquipmentManager( IEquipmentRepository equipmentRepository )
		{
			_equipmentRepository = equipmentRepository;
		}

		public async Task<List<EquipmentItem>> GetEquipmentItemsAsync()
		{
			List<EquipmentItem> equipment = await _equipmentRepository.GetEquipmentAsync().ConfigureAwait( false );

			foreach ( var equipmentItem in equipment )
			{
				UpdateNextServiceForAllMaintenanceItems( equipmentItem );
			}

			return equipment;
		}

		public async Task<Tuple<EquipmentItem, List<EquipmentValidationFailureType>>> GetEquipmentItemAsync( int id )
		{
			List<EquipmentValidationFailureType> validationFailures = new List<EquipmentValidationFailureType>();

			if ( !ValidationHelper.IsValidId( id ) )
			{
				validationFailures.Add( EquipmentValidationFailureType.InvalidEquipmentId );
			}

			EquipmentItem equipmentItem = await _equipmentRepository.GetEquipmentItemAsync( id ).ConfigureAwait( false );

			UpdateNextServiceForAllMaintenanceItems( equipmentItem );

			return Tuple.Create( equipmentItem, validationFailures );
		}

		public async Task<List<EquipmentValidationFailureType>> CreateEquipmentItemAsync( EquipmentItem equipmentItem )
		{
			List<EquipmentValidationFailureType> validationFailures = ValidateEquipmentItem( equipmentItem );

			if ( validationFailures.Any() )
			{
				return validationFailures;
			}

			equipmentItem = SanitizeValidatedEquipmentItem( equipmentItem );

			UpdateNextServiceForAllMaintenanceItems( equipmentItem );

			bool response = await _equipmentRepository.CreateEquipmentItemAsync( equipmentItem ).ConfigureAwait( false );

			if ( !response )
			{
				validationFailures.Add( EquipmentValidationFailureType.GenericErrorOccurred );
			}

			return validationFailures;
		}

		public async Task<List<EquipmentValidationFailureType>> UpdateEquipmentItemAsync( EquipmentItem equipmentItem )
		{
			List<EquipmentValidationFailureType> validationFailures = ValidateEquipmentItem( equipmentItem );

			if ( validationFailures.Any() )
			{
				return validationFailures;
			}

			equipmentItem = SanitizeValidatedEquipmentItem( equipmentItem );
			UpdateNextServiceForAllMaintenanceItems( equipmentItem );

			bool response = await _equipmentRepository.UpdateEquipmentItemAsync( equipmentItem ).ConfigureAwait( false );

			if ( !response )
			{
				validationFailures.Add( EquipmentValidationFailureType.GenericErrorOccurred );
			}

			return validationFailures;
		}

		public async Task<List<EquipmentValidationFailureType>> DeleteEquipmentItemAsync( int id )
		{
			List<EquipmentValidationFailureType> validationFailures = new List<EquipmentValidationFailureType>();

			if ( !ValidationHelper.IsValidId( id ) )
			{
				validationFailures.Add( EquipmentValidationFailureType.InvalidEquipmentId );

				return validationFailures;
			}

			bool response = await _equipmentRepository.DeleteEquipmentItemAsync( id ).ConfigureAwait( false );

			if ( !response )
			{
				validationFailures.Add( EquipmentValidationFailureType.GenericErrorOccurred );
			}

			return validationFailures;
		}

		//private bool IsValidId( int id )
		//{
		//    return id > 0;
		//}

		/// <summary>
		/// Calculates the next scheduled date for a maintenance item.
		/// </summary>
		/// <param name="maintenanceItem"></param>
		public DateTime? GetNextService( MaintenanceItem maintenanceItem )
		{
			if ( maintenanceItem == null ||
				maintenanceItem.MaintenanceLog == null ||
				!maintenanceItem.MaintenanceLog.Any() ||
				maintenanceItem.Schedule == null )
			{
				return null;
			}

			DateTime? lastServicedDate = maintenanceItem.MaintenanceLog.OrderBy( p => p.Date ).LastOrDefault( p => p.LogType != MaintenanceLogType.Stopped )?.Date;

			if ( lastServicedDate == null )
			{
				return null;
			}

			return GetNextService( lastServicedDate, maintenanceItem.Schedule.RecuringType );
		}

		private DateTime? GetNextService( DateTime? lastServicedDate, RecuringType recuringType ) => recuringType switch
		{
			RecuringType.Daily => lastServicedDate?.AddDays( 1 ),
			RecuringType.Weekly => lastServicedDate?.AddDays( 7 ),
			RecuringType.Biweekly => lastServicedDate?.AddDays( 14 ),
			RecuringType.Monthly => lastServicedDate?.AddMonths( 1 ),
			RecuringType.Bimonthly => lastServicedDate?.AddMonths( 2 ),
			RecuringType.Quarterly => lastServicedDate?.AddMonths( 3 ),
			RecuringType.Semiannually => lastServicedDate?.AddMonths( 6 ),
			RecuringType.Annually => lastServicedDate?.AddYears( 1 ),
			RecuringType.Biannually => lastServicedDate?.AddYears( 2 ),
			_ => null
		};

		private List<EquipmentValidationFailureType> ValidateEquipmentItem( EquipmentItem equipmentItem )
		{
			List<EquipmentValidationFailureType> validationFailures = new List<EquipmentValidationFailureType>();

			if ( equipmentItem == null )
			{
				validationFailures.Add( EquipmentValidationFailureType.MissingEquipmentItem );

				return validationFailures;
			}

			if ( string.IsNullOrWhiteSpace( equipmentItem.Name ) )
			{
				validationFailures.Add( EquipmentValidationFailureType.MissingEquipmentName );
			}

			return validationFailures;
		}

		private EquipmentItem SanitizeValidatedEquipmentItem( EquipmentItem equipmentItem )
		{
			if ( string.IsNullOrWhiteSpace( equipmentItem.Notes ) )
			{
				equipmentItem.Notes = null;
			}

			return equipmentItem;
		}

		private void UpdateNextServiceForAllMaintenanceItems( EquipmentItem equipmentItem )
		{
			foreach ( var maintenanceItem in equipmentItem.MaintenanceItems )
			{
				maintenanceItem.NextService = GetNextService( maintenanceItem );
			}
		}
	}
}
