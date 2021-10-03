using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NormStarr.ErrorHandling
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse()
        {
            
        }
        public ApiErrorResponse(int? statusCode,string message = null)
        {   
            statusCode = StatusCodes;
            message = Message ?? ErrroMessageResponse(StatusCodes);
        }

        private string ErrroMessageResponse(int? statusCode)
        {
           return statusCode switch 
           {
               400 => "You made a bad request!",
               401 => "You are not authorized!",
               404 => "What you are looking for does not exist!",
               500 => "The Errors that are made are from our end!",
               _=> null
           };
        }

        public int? StatusCodes { get; set; }
        public string  Message { get; set; }
    }
   
}