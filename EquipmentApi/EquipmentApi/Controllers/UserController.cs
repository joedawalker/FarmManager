using EquipmentApi.Classes;
using EquipmentApi.Models;
using Framework.NetCore.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentApi.Controllers
{
    [Produces( "application/json" )]
    [Route( "api/[controller]" )]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserManager _userManager;

        public UserController( IUserManager userManager )
        {
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<User> users = await _userManager.GetUsersAsync().ConfigureAwait( false );

            return new JsonResult( users );
        }

        // GET: api/Users/5
        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( int id )
        {
            var response = await _userManager.GetUserAsync( id ).ConfigureAwait( false );

            if ( response.Item2.Any() )
            {
                return ConvertValidationFailureToResponse( response.Item2 );
            }

            return new JsonResult( response.Item1 );
        }

        [HttpGet( "email/{email}" )]
        public async Task<IActionResult> Get( string email )
        {
            var response = await _userManager.GetUserAsync( email ).ConfigureAwait( false );

            if ( response.Item2.Any() )
            {
                return ConvertValidationFailureToResponse( response.Item2 );
            }

            return new JsonResult( response.Item1 );
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post( [FromBody] User user )
        {
            List<UserValidationFailureType> validationFailures = await _userManager.CreateUserAsync( user ).ConfigureAwait( false );

            if ( validationFailures != null && validationFailures.Any() )
            {
                return ConvertValidationFailureToResponse( validationFailures );
            }

            return Ok( $"User {user.FirstName} was created successfully." );
        }

        // PUT: api/Users/5
        [HttpPut]
        public async Task<IActionResult> Put( [FromBody] User user )
        {
            List<UserValidationFailureType> validationFailures = await _userManager.UpdateUserAsync( user ).ConfigureAwait( false );

            if ( validationFailures != null && validationFailures.Any() )
            {
                return ConvertValidationFailureToResponse( validationFailures );
            }

            return JsonResultHelper.OkResponse( new SuccessModel( "User updated successfully" ) );
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id )
        {
            List<UserValidationFailureType> validationFailures = await _userManager.DeleteUserAsync( id ).ConfigureAwait( false );

            if ( validationFailures.Any() )
            {
                return ConvertValidationFailureToResponse( validationFailures );
            }

            return JsonResultHelper.OkResponse( new SuccessModel( "User deleted successfully" ) );
        }

        private IActionResult ConvertValidationFailureToResponse( List<UserValidationFailureType> validationFailures )
        {
            foreach ( var validationFailure in validationFailures )
            {
                switch ( validationFailure )
                {
                    case UserValidationFailureType.UserIsNull:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "User cannot be null.", "Please provide a valid user." ) );
                    case UserValidationFailureType.UserNotFound:
                        return JsonResultHelper.NotFoundResponse( new ErrorModel( "User not found.", "Verify user id and try again." ) );
                    case UserValidationFailureType.InvalidId:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Invalid id.", "Id's must be greater than 0." ) );
                    case UserValidationFailureType.FirstNameMissing:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Missing first_name.", "Please provide a first_name." ) );
                    case UserValidationFailureType.LastNameMissing:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Missing last_name.", "Please provide a last_name." ) );
                    case UserValidationFailureType.EmailMissing:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Missing email.", "Please provide a valid email." ) );
                    case UserValidationFailureType.EmailBadFormat:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Invalid email.", "Please provide a valid email." ) );
                    case UserValidationFailureType.EmailAlreadyInUse:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Email provided is already in use.", "Please provide a different email." ) );
                    case UserValidationFailureType.PasswordMissing:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Missing password.", "Please provide a valid password." ) );
                    case UserValidationFailureType.PasswordBadFormat:
                        return JsonResultHelper.BadRequestResponse( new ErrorModel( "Invalid password.", "Please provide a valid password." ) );
                    case UserValidationFailureType.GenericError:
                        return JsonResultHelper.ServerErrorResponse( new ErrorModel( "Server error occurred.", "Check your request and try again." ) );
                    default:
                        break;
                }
            }

            throw new HttpResponseException();
        }
    }
}
