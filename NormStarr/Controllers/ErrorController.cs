
using Microsoft.AspNetCore.Mvc;
using NormStarr.ErrorHandling;

namespace NormStarr.Controllers
{
    [Route("/errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController:BaseController
    {
        public IActionResult Error(int code)
        {
            return new OkObjectResult(new ApiErrorResponse(code));
        }
    }
}