using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly DemandDbContext _demandDbContext;

        public CustomerService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }

        public List<CustomerViewModel> GetCustomers(int startPage, int pageSize, out int rowCount)
        {
            rowCount = _demandDbContext.Roles.Count();
            var customers = _demandDbContext.Customers.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return AutoMapper.Mapper.Map<List<CustomerViewModel>>(customers);
        }
    }
}
