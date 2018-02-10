using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers.API
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    public class DemandController : Controller
    {
        [HttpGet]
        [Route("api/getactivedemands")]
        public IActionResult GetActiveDemands()
        {
            return Json(new{A=1,B=2});
        }

    }
}