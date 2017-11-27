using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface IRoleService
    {
        List<RoleViewModel> GetRoles(int startPage, int pageSize, out int rowCount);

        List<int> GetMenuIdsByRoleId(int roleId);

        RoleViewModel GetRoleById(int id);

        bool AddRole(RoleViewModel roleViewModel, out string reason);

        bool UpdateRole(RoleViewModel roleViewModel, out string reason);

        void DeleteRole(int id);

        void DeleteRoles(List<int> ids);

        List<RoleViewModel> GetAllRoles();
    }
}
