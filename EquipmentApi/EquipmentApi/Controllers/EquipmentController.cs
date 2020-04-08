using EquipmentApi.Classes;
using EquipmentApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentApi.Controllers
{
	[Produces( "application/json" )]
	[Route( "api/[controller]" )]
	[ApiController]
	public class EquipmentController : ControllerBase
	{
		private readonly IEquipmentManager _equipmentManager;

		public EquipmentController( IEquipmentManager equipmentManager )
		{
			_equipmentManager = equipmentManager;
		}

		// GET: api/Equipment
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			List<EquipmentItem> equipment = await _equipmentManager.GetEquipmentItemsAsync().ConfigureAwait( false );

			if ( equipment == null || !equipment.Any() )
			{
				return JsonResultHelper.NotFoundResponse( new ErrorModel( "No equipment items were found.", "Create equipment and try again." ) );
			}

			return JsonResultHelper.OkResponse( equipment );
		}

		// GET: api/Equipment/5
		[HttpGet( "{id}" )]
		public async Task<IActionResult> Get( int id )
		{
			Tuple<EquipmentItem, List<EquipmentValidationFailureType>> equipmentResponse = await _equipmentManager.GetEquipmentItemAsync( id ).ConfigureAwait( false );

			if ( equipmentResponse.Item2.Any() )
			{
				return ConvertValidationFailureToResponse( equipmentResponse.Item2 );
			}

			return JsonResultHelper.OkResponse( equipmentResponse.Item1 );
		}

		// POST: api/Equipment
		[HttpPost]
		public async Task<IActionResult> Post( [FromBody] EquipmentItem newItem )
		{
			var validationFailures = await _equipmentManager.CreateEquipmentItemAsync( newItem ).ConfigureAwait( false );

			if ( validationFailures.Any() )
			{
				return ConvertValidationFailureToResponse( validationFailures );
			}

			return JsonResultHelper.CreatedResponse( new { success = $"{bool.TrueString}", message = "The equipment_item created successfully." } );
		}

		// PUT: api/Equipment/5
		[HttpPut]
		public async Task<IActionResult> Put( [FromBody] EquipmentItem equipmentItem )
		{
			var validationFailures = await _equipmentManager.UpdateEquipmentItemAsync( equipmentItem ).ConfigureAwait( false );

			if ( validationFailures.Any() )
			{
				return ConvertValidationFailureToResponse( validationFailures );
			}

			return JsonResultHelper.OkResponse( new { success = $"{bool.TrueString}", message = "The equipment_item was replaced successfuly." } );
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete( "{id}" )]
		public async Task<IActionResult> Delete( int id )
		{
			List<EquipmentValidationFailureType> validationFailures = await _equipmentManager.DeleteEquipmentItemAsync( id ).ConfigureAwait( false );

			if ( validationFailures.Any() )
			{
				return ConvertValidationFailureToResponse( validationFailures );
			}

			return JsonResultHelper.OkResponse( new SuccessModel( "The equipment_item was deleted successfuly." ) );
		}

		private IActionResult ConvertValidationFailureToResponse( List<EquipmentValidationFailureType> validationFailures )
		{
			StringBuilder responseBuilder = new StringBuilder();

			foreach ( EquipmentValidationFailureType failure in validationFailures.OrderBy( v => v.GetTypeCode() ) )
			{
				switch ( failure )
				{
					case EquipmentValidationFailureType.MissingEquipmentItem:
						return JsonResultHelper.BadRequestResponse( new ErrorModel( "No equipment_item was included in the request.", "Make sure you have your request body formatted correctly." ) );
					case EquipmentValidationFailureType.MissingEquipmentId:
						return JsonResultHelper.BadRequestResponse( new ErrorModel( "Request is missing an equipment_item id.", "Add a valid equipment item id to your request." ) );
					case EquipmentValidationFailureType.InvalidEquipmentId:
						return JsonResultHelper.BadRequestResponse( new ErrorModel( "Invalid equipment_item id.", "Id's must be greater than 0." ) );
					case EquipmentValidationFailureType.MissingEquipmentName:
						return JsonResultHelper.BadRequestResponse( new ErrorModel( "Missing equipment_item name", "Try again with a valid name" ) );
					case EquipmentValidationFailureType.GenericErrorOccurred:
					default:
						break;
				}
			}

			return JsonResultHelper.ServerErrorResponse( new ErrorModel( "Unkown error occurred.", "Check your request and try again." ) );
		}
	}
}
