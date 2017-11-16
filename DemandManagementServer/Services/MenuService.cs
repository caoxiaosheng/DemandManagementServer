﻿using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.DAL;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;

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
            var newMenu = AutoMapper.Mapper.Map<Menu>(menuViewModel);
            EntityToEntity(newMenu, menu);
            _demandDbContext.SaveChanges();
            return true;
        }
        
        public void DeleteMenu(int id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteMenus(List<int> ids)
        {
            throw new System.NotImplementedException();
        }

        private void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        {
            foreach (var mItem in typeof(T).GetProperties())
            {
                mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
            }
        }
    }
}
