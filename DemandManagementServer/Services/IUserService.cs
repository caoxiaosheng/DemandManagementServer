using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface IUserService
    {
        User CheckUser(string userName, string password);
        List<UserViewModel> GetUsers(int startPage, int pageSize, out int rowCount);
    }
}
