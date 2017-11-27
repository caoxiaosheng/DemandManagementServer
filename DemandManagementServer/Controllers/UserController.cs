using System;
using DemandManagementServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUsers(int startPage, int pageSize)
        {
            var result = _userService.GetUsers(startPage, pageSize, out var rowCount);
            return Json(new
            {
                users = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }

        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            return Json(user);
        }
    }
}