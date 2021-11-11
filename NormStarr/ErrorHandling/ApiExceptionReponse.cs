using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NormStarr.ErrorHandling
{
    public class ApiExceptionReponse : ApiErrorResponse
    {
        public ApiExceptionReponse(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}