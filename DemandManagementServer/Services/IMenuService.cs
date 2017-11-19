using System.Collections.Generic;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface IMenuService
    {
        List<MenuViewModel> GetMenus(int startPage, int pageSize, out int rowCount);

        MenuViewModel GetMenuById(int id);

        bool AddMenu(MenuViewModel menuViewModel,out string reason);

        bool UpdateMenu(MenuViewModel menuViewModel, out string reason);

        void DeleteMenu(int id);

        void DeleteMenus(List<int> ids);

        List<MenuViewModel> GetAllMenus();
    }
}
