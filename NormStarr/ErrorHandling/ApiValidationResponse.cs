using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NormStarr.ErrorHandling
{
    //This Controller will Validate Error Response Users Create when filling out a form!
    public class ApiValidationResponse : ApiErrorResponse
    {
        public ApiValidationResponse():base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}