using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
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

        public IActionResult GetDemandById(int id)
        {
            var demand = _service.GetDemandById(id);
            return Json(demand);
        }

        public IActionResult AddDemand(DemandViewModelEdit demandViewModelEdit)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            var result = _service.AddDemand(demandViewModelEdit, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }
    }
}