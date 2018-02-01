using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.Models;
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
            rowCount = _demandDbContext.Customers.Count();
            var customers = _demandDbContext.Customers.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return AutoMapper.Mapper.Map<List<CustomerViewModel>>(customers);
        }

        public CustomerViewModel GetCustomerById(int id)
        {
            var customer = _demandDbContext.Customers.SingleOrDefault(item => item.Id == id);
            return AutoMapper.Mapper.Map<CustomerViewModel>(customer);
        }

        public bool AddCustomer(CustomerViewModel customerViewModel, out string reason)
        {
            reason = string.Empty;
            var newCustomer = AutoMapper.Mapper.Map<Customer>(customerViewModel);
            var customer = _demandDbContext.Customers.SingleOrDefault(item => item.Name == newCustomer.Name);
            if (customer != null)
            {
                reason = "已存在名称：" + newCustomer.Name;
                return false;
            }
            newCustomer.CreateTime=DateTime.Now;
            newCustomer.CustomerState = CustomerState.正常;
            _demandDbContext.Customers.Add(newCustomer);
            _demandDbContext.SaveChanges();
            return true;
        }

        public bool UpdateCustomer(CustomerViewModel customerViewModel, out string reason)
        {
            reason = string.Empty;
            var customer = _demandDbContext.Customers.SingleOrDefault(item => item.Id == customerViewModel.Id);
            if (customer == null)
            {
                reason = "未查找到该客户";
                return false;
            }
            //仅名称变了 才需要判断重复
            if (customerViewModel.Name != customer.Name)
            {
                var sameNameCustomer = _demandDbContext.Customers.SingleOrDefault(item => item.Name == customerViewModel.Name);
                if (sameNameCustomer != null)
                {
                    reason = "已存在名称：" + sameNameCustomer.Name;
                    return false;
                }
            }
            var newCustomer = AutoMapper.Mapper.Map<Customer>(customerViewModel);
            customer.Name = newCustomer.Name;
            customer.CreateTime=DateTime.Now;
            customer.CustomerPriority = newCustomer.CustomerPriority;
            customer.CustomerType = newCustomer.CustomerType;
            customer.Remarks = newCustomer.Remarks;
            _demandDbContext.SaveChanges();
            return true;
        }

        public void DeleteCustomers(List<int> ids)
        {
            foreach (var id in ids)
            {
                var customer = _demandDbContext.Customers.SingleOrDefault(item => item.Id == id);
                if (customer != null)
                {
                    customer.CustomerState=CustomerState.封禁;
                }
            }
            _demandDbContext.SaveChanges();
        }

        public List<CustomerViewModel> GetAllCustomers()
        {
            var customers = _demandDbContext.Customers.OrderBy(item => item.Id);
            return AutoMapper.Mapper.Map<List<CustomerViewModel>>(customers);
        }
    }
}
