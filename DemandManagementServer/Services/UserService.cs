using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.Models;

namespace DemandManagementServer.Services
{
    public class UserService:IUserService
    {
        private readonly DemandDbContext _demandDbContext;

        public UserService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }
        
        public User CheckUser(string userName, string password)
        {
            return _demandDbContext.Users.FirstOrDefault(it => it.UserName == userName && it.Password == password);
        }
    }
}
