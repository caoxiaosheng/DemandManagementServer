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
            return Json(new{A=1,B=2});
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