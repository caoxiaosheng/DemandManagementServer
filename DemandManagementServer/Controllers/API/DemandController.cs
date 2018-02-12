using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers.API
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    public class DemandController : Controller
    {
        private readonly IDemandService _service;

        public DemandController(IDemandService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/getactivedemands")]
        public IActionResult GetActiveDemands()
        {
            var userName = HttpContext.User.Identity.Name;
            var result = _service.GetActiveDemands(userName);
            return Json(new
            {
                demands = result
            });
        }

        [HttpGet]
        [Route("api/getenddemands")]
        public IActionResult GetEndDemands()
        {
            var userName = HttpContext.User.Identity.Name;
            var result = _service.GetEndDemands(userName);
            return Json(new
            {
                demands = result
            });
        }

        [HttpPost]
        [Route("api/adddemand")]
        public IActionResult AddDemand([FromBody] DemandViewModelEditAPI demandViewModelEditApi)
        {
            var result = _service.AddDemand(demandViewModelEditApi, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }

    }
}