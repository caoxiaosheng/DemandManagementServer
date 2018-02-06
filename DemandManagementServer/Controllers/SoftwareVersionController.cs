using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class SoftwareVersionController : Controller
    {
        private readonly ISoftwareVersionService _service;

        public SoftwareVersionController(ISoftwareVersionService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSoftwareVersions(int startPage, int pageSize)
        {
            var result = _service.GetSoftwareVersions(startPage, pageSize, out var rowCount);
            return Json(new
            {
                softwareVersions = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }

        public IActionResult GetSoftwareVersionById(int id)
        {
            var softwareVersion = _service.GetSoftwareVersionById(id);
            return Json(softwareVersion);
        }

        public IActionResult AddSoftwareVersion(SoftwareVersionViewModel softwareVersionView)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            var result = _service.AddSoftwareVersion(softwareVersionView, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }

        public IActionResult EditSoftwareVersion(SoftwareVersionViewModel softwareVersionView)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            var result = _service.UpdateSoftwareVersion(softwareVersionView, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }

        public void DeleteSingle(int id)
        {
            _service.DeleteSoftwareVersions(new List<int>() { id });
        }

        public void Release(int id)
        {
            _service.ReleaseSoftwareVersions(id);
        }
    }
}