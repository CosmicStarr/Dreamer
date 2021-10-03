
using Microsoft.AspNetCore.Mvc;
using NormStarr.ErrorHandling;

namespace NormStarr.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorHandlingController:BaseController
    {
        public IActionResult ErrorsAgain(int code)
        {
            return new OkObjectResult(new ApiErrorResponse(code));
        }
    }
}