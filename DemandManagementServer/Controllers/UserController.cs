using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
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

        public IActionResult AddUser(UserViewModel userViewModel,string roleIds)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            try
            {
                List<UserRoleViewModel> userRoleViewModels = new List<UserRoleViewModel>();
                foreach (var id in roleIds.Select2StringToList())
                {
                    userRoleViewModels.Add(new UserRoleViewModel()
                    {
                        UserId = userViewModel.Id,
                        RoleId = id
                    });
                }
                userViewModel.UserRoles = userRoleViewModels;
                var result = _userService.AddUser(userViewModel, out var reason);
                return Json(new
                {
                    result = result,
                    reason = reason
                });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    result = false,
                    reason = exception.Message
                });
            }
        }

        public IActionResult EditUser(UserViewModel userViewModel, string roleIds)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            try
            {
                List<UserRoleViewModel> userRoleViewModels = new List<UserRoleViewModel>();
                foreach (var id in roleIds.Select2StringToList())
                {
                    userRoleViewModels.Add(new UserRoleViewModel()
                    {
                        UserId = userViewModel.Id,
                        RoleId = id
                    });
                }
                userViewModel.UserRoles = userRoleViewModels;
                var result = _userService.UpdateUser(userViewModel, out var reason);
                return Json(new
                {
                    result = result,
                    reason = reason
                });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    result = false,
                    reason = exception.Message
                });
            }
        }

        public void DeleteSingle(int id)
        {
            _userService.DeleteUser(id);
        }

        public void DeleteMulti(List<int> ids)
        {
            _userService.DeleteUsers(ids);
        }

        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Json(users);
        }

        public IActionResult GetCurrentUser()
        {
            var user = _userService.GetUserByUserName(HttpContext.User.Identity.Name);
            return Json(user);
        }
    }
}