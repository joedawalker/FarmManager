using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Framework.NetCore.Web
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = (int)HttpStatusCode.InternalServerError;
        public object Value { get; set; } = "Something went wrong during your request. Please try again.";
    }
}
