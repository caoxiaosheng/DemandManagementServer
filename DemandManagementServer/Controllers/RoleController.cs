using System;
using System.Collections.Generic;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace DemandManagementServer.Controllers
{
    [Authorize(AuthenticationSchemes= "Cookies")]
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

        public IActionResult GetMenuIdsByRoleId(int roleId)
        {
            var menuIds = _roleService.GetMenuIdsByRoleId(roleId);
            return Json(menuIds);
        }

        public IActionResult GetRoleById(int id)
        {
            var role = _roleService.GetRoleById(id);
            return Json(role);
        }

        public IActionResult AddRole(RoleViewModel roleViewModel,string menuIds)
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
                List<RoleMenuViewModel> roleMenuViewModels = new List<RoleMenuViewModel>();
                foreach (var id in menuIds.Select2StringToList())
                {
                    roleMenuViewModels.Add(new RoleMenuViewModel()
                    {
                        RoleId = roleViewModel.Id,
                        MenuId = id
                    });
                }
                roleViewModel.RoleMenus = roleMenuViewModels;
                var result = _roleService.AddRole(roleViewModel, out var reason);
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

        public IActionResult EditRole(RoleViewModel roleViewModel, string menuIds)
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
                List<RoleMenuViewModel> roleMenuViewModels = new List<RoleMenuViewModel>();
                foreach (var id in menuIds.Select2StringToList())
                {
                    roleMenuViewModels.Add(new RoleMenuViewModel()
                    {
                        RoleId = roleViewModel.Id,
                        MenuId = id
                    });
                }
                roleViewModel.RoleMenus = roleMenuViewModels;
                var result = _roleService.UpdateRole(roleViewModel, out var reason);
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
            _roleService.DeleteRole(id);
        }

        public void DeleteMulti(List<int> ids)
        {
            _roleService.DeleteRoles(ids);
        }

        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetAllRoles();
            return Json(roles);
        }
    }
}