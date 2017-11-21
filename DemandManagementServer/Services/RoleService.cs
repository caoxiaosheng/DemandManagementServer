using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.DAL;
using DemandManagementServer.ViewModels;

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
    }
}
