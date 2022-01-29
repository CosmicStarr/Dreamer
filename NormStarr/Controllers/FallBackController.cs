using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NormStarr.Controllers
{
    [Route("[controller]")]
    public class FallBackController : Controller
    {

        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","index.html"),"text/HTML");
        }

     }
}