using Microsoft.AspNetCore.Mvc;
using Framework.NetCore.Web;
using System.Collections.Generic;
using UserService;
using System.Linq;
using System.Text.Json;

namespace EquipmentManager.Controllers
{
    [Produces( "application/json" )]
    [Route( "api/[controller]" )]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserManager _userManager;

        public UsersController( IUserManager userManager )
        {
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public JsonResult Get()
        {
            List<User> users = _userManager.GetUsers();

            return new JsonResult( users );
        }

        // GET: api/Users/5
        [HttpGet( "{id}" )]
        public JsonResult Get( int id )
        {
            return new JsonResult( _userManager.GetUser( id ) );
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult Post( [FromBody] string firstName, string lastName, string email, string password )
        {
            List<UserValidationFailureType> userValidationFailures = _userManager.CreateUser( firstName, lastName, email, password );

            if ( userValidationFailures != null && userValidationFailures.Any() )
            {
                foreach ( var validationFailure in userValidationFailures )
                {
                    switch ( validationFailure )
                    {
                        case UserValidationFailureType.FirstNameMissing:
                            return BadRequest( "Missing user's first name." );
                        case UserValidationFailureType.LastNameMissing:
                            return BadRequest( "Missing user's last name." );
                        case UserValidationFailureType.EmailMissing:
                            return BadRequest( "Missing user's email address." );
                        case UserValidationFailureType.EmailBadFormat:
                            return BadRequest( "Invalid email format." );
                        case UserValidationFailureType.PasswordMissing:
                            return BadRequest( "Missing user's password." );
                        case UserValidationFailureType.PasswordBadFormat:
                            return BadRequest( "Incorrect password format." );
                        default:
                            throw new HttpResponseException();
                    } 
                }
            }

            return Ok( $"User {firstName} was created successfully." );
        }

        // PUT: api/Users/5
        [HttpPut( "{id}" )]
        public void Put( int id, [FromBody] string value )
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete( "{id}" )]
        public void Delete( int id )
        {
        }
    }
}
