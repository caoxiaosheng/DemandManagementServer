using System.Collections.Generic;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface IMenuService
    {
        List<MenuViewModel> GetMenus(int startPage, int pageSize, out int rowCount);
    }
}
