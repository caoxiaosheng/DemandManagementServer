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
        UserViewModel GetUserById(int id);
        bool AddUser(UserViewModel userViewModel, out string reason);
        bool UpdateUser(UserViewModel userViewModel, out string reason);
        void DeleteUser(int id);
        void DeleteUsers(List<int> ids);
        List<UserViewModel> GetAllUsers();
        UserViewModel GetUserByUserName(string userName);
    }
}
