using System;
using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.DAL;
using DemandManagementServer.Extensions;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DemandManagementServer.Services
{
    public class RoleService:IRoleService
    {
        private readonly DemandDbContext _demandDbContext;

        public RoleService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }

        public List<RoleViewModel> GetRoles(int startPage, int pageSize, out int rowCount)
        {
            rowCount = _demandDbContext.Roles.Count();
            var roles = _demandDbContext.Roles.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return AutoMapper.Mapper.Map<List<RoleViewModel>>(roles);
        }

        public List<int> GetMenuIdsByRoleId(int roleId)
        {
            var menuIds = _demandDbContext.RoleMenus.Where(item => item.RoleId == roleId).Select(item=>item.MenuId).ToList();
            return menuIds;
        }

        public RoleViewModel GetRoleById(int id)
        {
            var role = _demandDbContext.Roles.Include(item => item.RoleMenus).FirstOrDefault(item => item.Id == id);
            return AutoMapper.Mapper.Map<RoleViewModel>(role);
        }

        public bool AddRole(RoleViewModel roleViewModel, out string reason)
        {
            reason = string.Empty;
            var newRole = AutoMapper.Mapper.Map<Role>(roleViewModel);
            var role = _demandDbContext.Roles.FirstOrDefault(item => item.Name == newRole.Name);
            if (role != null)
            {
                reason = "已存在名称：" + newRole.Name;
                return false;
            }
            newRole.CreateTime=DateTime.Now;
            _demandDbContext.Roles.Add(newRole);
            _demandDbContext.SaveChanges();
            return true;
        }

        public bool UpdateRole(RoleViewModel roleViewModel, out string reason)
        {
            reason = string.Empty;
            if (roleViewModel.Id == 1)
            {
                reason = "管理员角色不允许修改";
                return false;
            }
            var role = _demandDbContext.Roles.Include(item=>item.RoleMenus).FirstOrDefault(item => item.Id == roleViewModel.Id);
            if (role == null)
            {
                reason = "未查找到该角色";
                return false;
            }
            //仅名称变了 才需要判断重复
            if (roleViewModel.Name != role.Name)
            {
                var sameNameRole = _demandDbContext.Roles.FirstOrDefault(item => item.Name == roleViewModel.Name);
                if (sameNameRole != null)
                {
                    reason = "已存在名称：" + roleViewModel.Name;
                    return false;
                }
            }
            //foreach (var rolemenu in role.RoleMenus)
            //{
            //    _demandDbContext.RoleMenus.Remove(rolemenu);
            //}
            role.RoleMenus.Clear();
            _demandDbContext.SaveChanges();
            var newRole = AutoMapper.Mapper.Map<Role>(roleViewModel);
            EntityUpdateHelper.EntityToEntity(newRole, role);
            role.CreateTime=DateTime.Now;
            _demandDbContext.SaveChanges();
            return true;
        }

        public void DeleteRole(int id)
        {
            if (id == 1)
            {
                return;
            }
            var role = _demandDbContext.Roles.Include(item => item.RoleMenus).SingleOrDefault(item => item.Id == id);
            if (role == null)
            {
                return;
            }
            _demandDbContext.Roles.Remove(role);
            //角色删除 同步移除用户的绑定
            var userRoles = _demandDbContext.UserRoles.Where(item => item.RoleId == role.Id);
            _demandDbContext.UserRoles.RemoveRange(userRoles);
            _demandDbContext.SaveChanges();
        }

        public void DeleteRoles(List<int> ids)
        {
            foreach (var id in ids)
            {
                if (id == 1)
                {
                    continue;
                }
                var role = _demandDbContext.Roles.Include(item=>item.RoleMenus).SingleOrDefault(item => item.Id == id);
                if (role != null)
                {
                    _demandDbContext.Roles.Remove(role);
                    var userRoles = _demandDbContext.UserRoles.Where(item => item.RoleId == role.Id);
                    _demandDbContext.UserRoles.RemoveRange(userRoles);
                }
            }
            _demandDbContext.SaveChanges();
        }

        public List<RoleViewModel> GetAllRoles()
        {
            var roles = _demandDbContext.Roles.OrderBy(item => item.Id);
            return AutoMapper.Mapper.Map<List<RoleViewModel>>(roles);
        }
    }
}
