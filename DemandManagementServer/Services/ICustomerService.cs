using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface ICustomerService
    {
        List<CustomerViewModel> GetCustomers(int startPage, int pageSize, out int rowCount);
    }
}
