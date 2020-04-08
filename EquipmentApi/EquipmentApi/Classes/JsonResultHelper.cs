using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentApi.Classes
{
    public static class JsonResultHelper
    {
        #region 2xx Responses
        public static IActionResult OkResponse( object value )
        {
            return CreateJsonResult( value, StatusCodes.Status200OK );
        }

        public static IActionResult CreatedResponse( object value )
        {
            return CreateJsonResult( value, StatusCodes.Status201Created );
        }
        #endregion

        #region 4xx Responses
        public static IActionResult UnauthorizedResponse( object value )
        {
            return CreateJsonResult( value, StatusCodes.Status401Unauthorized );
        }

        public static IActionResult BadRequestResponse( object value )
        {
            return CreateJsonResult( value, StatusCodes.Status400BadRequest );
        }

        public static IActionResult NotFoundResponse( object value )
        {
            return CreateJsonResult( value, StatusCodes.Status404NotFound );
        }
        #endregion

        #region 5xx Responses
        public static IActionResult ServerErrorResponse( object value )
        {
            return CreateJsonResult( value, StatusCodes.Status500InternalServerError );
        }
        #endregion

        private static JsonResult CreateJsonResult( object value, int statusCode )
        {
            return new JsonResult( value )
            {
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }
    }
}
