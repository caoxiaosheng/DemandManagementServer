using System;
using DemandManagementServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRoles(int startPage, int pageSize)
        {
            var result = _roleService.GetRoles(startPage, pageSize, out var rowCount);
            return Json(new
            {
                roles = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }
    }
}