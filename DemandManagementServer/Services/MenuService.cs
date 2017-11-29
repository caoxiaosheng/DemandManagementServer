using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.DAL;
using DemandManagementServer.Extensions;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DemandManagementServer.Services
{
    public class MenuService:IMenuService
    {
        private readonly DemandDbContext _demandDbContext;

        public MenuService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }

        public List<MenuViewModel> GetMenus(int startPage, int pageSize, out int rowCount)
        {
            rowCount = _demandDbContext.Menus.Count();
            var menus=_demandDbContext.Menus.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return AutoMapper.Mapper.Map<List<MenuViewModel>>(menus);
        }

        public MenuViewModel GetMenuById(int id)
        {
            var menu = _demandDbContext.Menus.FirstOrDefault(item => item.Id == id);
            return AutoMapper.Mapper.Map<MenuViewModel>(menu);
        }

        public bool AddMenu(MenuViewModel menuViewModel, out string reason)
        {
            reason = string.Empty;
            var newMenu = AutoMapper.Mapper.Map<Menu>(menuViewModel);
            var menu = _demandDbContext.Menus.FirstOrDefault(item => item.Name == newMenu.Name);
            if (menu != null)
            {
                reason = "已存在名称：" + newMenu.Name;
                return false;
            }
            _demandDbContext.Menus.Add(newMenu);
            //新增的功能 自动附加到管理员
            var adminRole = _demandDbContext.Roles.Single(item => item.Id == 1);
            _demandDbContext.RoleMenus.Add(new RoleMenu() {Menu = newMenu, Role = adminRole});
            _demandDbContext.SaveChanges();
            return true;
        }

        public bool UpdateMenu(MenuViewModel menuViewModel,out string reason)
        {
            reason = string.Empty;
            var menu = _demandDbContext.Menus.FirstOrDefault(item => item.Id == menuViewModel.Id);
            if (menu == null)
            {
                reason = "未查找到该功能菜单";
                return false;
            }
            //仅名称变了 才需要判断重复
            if (menuViewModel.Name != menu.Name)
            {
                var sameNameMenu = _demandDbContext.Menus.FirstOrDefault(item => item.Name == menuViewModel.Name);
                if (sameNameMenu != null)
                {
                    reason = "已存在名称：" + sameNameMenu.Name;
                    return false;
                }
            }
            var newMenu = AutoMapper.Mapper.Map<Menu>(menuViewModel);
            EntityUpdateHelper.EntityToEntity(newMenu, menu);
            _demandDbContext.SaveChanges();
            return true;
        }
        
        public void DeleteMenu(int id)
        {
            var menu = _demandDbContext.Menus.FirstOrDefault(item => item.Id == id);
            if (menu == null)
            {
                return;
            }
            _demandDbContext.Menus.Remove(menu);
            //功能删除 同步移除角色的绑定
            var roleMenus = _demandDbContext.RoleMenus.Where(item => item.MenuId == menu.Id);
            _demandDbContext.RoleMenus.RemoveRange(roleMenus);
            _demandDbContext.SaveChanges();
        }

        public void DeleteMenus(List<int> ids)
        {
            foreach (var id in ids)
            {
                var menu = _demandDbContext.Menus.FirstOrDefault(item => item.Id == id);
                if (menu != null)
                {
                    _demandDbContext.Menus.Remove(menu);
                    var roleMenus = _demandDbContext.RoleMenus.Where(item => item.MenuId == menu.Id);
                    _demandDbContext.RoleMenus.RemoveRange(roleMenus);
                }
            }
            _demandDbContext.SaveChanges();
        }

        public List<MenuViewModel> GetAllMenus()
        {
            var menus = _demandDbContext.Menus.OrderBy(item => item.Id);
            return AutoMapper.Mapper.Map<List<MenuViewModel>>(menus);
        }

        public List<MenuViewModel> GetMenusByUserName(string userName)
        {
            var allMenus= _demandDbContext.Menus.OrderBy(item => item.Id);
            
            var user = _demandDbContext.Users.Include(item => item.UserRoles).ThenInclude(item => item.Role)
                .ThenInclude(item => item.RoleMenus).ThenInclude(item => item.Menu)
                .SingleOrDefault(item => item.UserName == userName);
            if (user == null)
            {
                return new List<MenuViewModel>();
            }
            if (user.Id==1)
            {
                return AutoMapper.Mapper.Map<List<MenuViewModel>>(allMenus);
            }
            List<Menu> selectedMenus=new List<Menu>();
            foreach (var userRole in user.UserRoles)
            {
                selectedMenus=selectedMenus.Union(userRole.Role.RoleMenus.Select(item => item.Menu)).ToList();
            }
            return AutoMapper.Mapper.Map<List<MenuViewModel>>(selectedMenus.OrderBy(item=>item.Id));
        }
    }
}
