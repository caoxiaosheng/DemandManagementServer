using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult GetRoles(int startPage, int pageSize)
        //{
        //    //var result = _menuService.GetMenus(startPage, pageSize, out var rowCount);
        //    //return Json(new
        //    //{
        //    //    menus = result,
        //    //    rowsCount = rowCount,
        //    //    pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
        //    //});
        //}
    }
}