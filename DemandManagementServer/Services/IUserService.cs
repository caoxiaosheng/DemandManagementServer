using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Models;

namespace DemandManagementServer.Services
{
    public interface IUserService
    {
        User CheckUser(string userName, string password);
    }
}
