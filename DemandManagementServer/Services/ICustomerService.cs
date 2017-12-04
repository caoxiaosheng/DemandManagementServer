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
        CustomerViewModel GetCustomerById(int id);
        bool AddCustomer(CustomerViewModel customerViewModel, out string reason);
        bool UpdateCustomer(CustomerViewModel customerViewModel, out string reason);
        void DeleteCustomers(List<int> ids);
    }
}
