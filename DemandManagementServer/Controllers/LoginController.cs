using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.CheckUser(loginViewModel.UserName, loginViewModel.Password);
                if (user != null)
                {
                    HttpContext.Session.SetObjectAsJson("CurrentUser",user);
                    return RedirectToAction("Index", "Management");
                }
                ModelState.AddModelError("","用户名或密码错误");
                ModelState.AddModelError("", "用户名或密码错误2");
                return View();
            }
            return View(loginViewModel);
        }
    }
}