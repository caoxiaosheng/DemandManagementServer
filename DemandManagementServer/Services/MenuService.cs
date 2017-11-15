using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.DAL;
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
    }
}
