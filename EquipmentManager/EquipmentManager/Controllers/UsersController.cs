using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserService;
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

            string json = JsonSerializer.Serialize( users );

            return new JsonResult( users );
        }

        // GET: api/Users/5
        [HttpGet( "{id}" )]
        public string Get( int id )
        {
            return "value";
        }

        // POST: api/Users
        [HttpPost]
        public void Post( [FromBody] string value )
        {
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
