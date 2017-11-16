using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetMenus(int startPage, int pageSize)
        {
            var result = _menuService.GetMenus(startPage, pageSize, out var rowCount);
            return Json(new
            {
                menus = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }

        public IActionResult GetMenuById(int id)
        {
            var menu = _menuService.GetMenuById(id);
            return Json(menu);
        }

        public IActionResult AddMenu(MenuViewModel menuViewModel)
        {
            var result = _menuService.AddMenu(menuViewModel,out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }

        public IActionResult EditMenu(MenuViewModel menuViewModel)
        {
            var result = _menuService.UpdateMenu(menuViewModel, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }
    }
}