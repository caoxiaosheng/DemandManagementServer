using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    public class DemandController : Controller
    {
        private readonly IDemandService _service;

        public DemandController(IDemandService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetDemands(int startPage, int pageSize)
        {
            var result = _service.GetDemands(startPage, pageSize, out var rowCount);
            return Json(new
            {
                demands = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }
    }
}