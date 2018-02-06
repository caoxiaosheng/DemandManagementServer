using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Extensions;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCustomers(int startPage, int pageSize)
        {
            var result = _customerService.GetCustomers(startPage, pageSize, out var rowCount);
            return Json(new
            {
                customers = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }

        public IActionResult GetCustomerById(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            return Json(customer);
        }

        public IActionResult AddCustomer(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            var result = _customerService.AddCustomer(customerViewModel, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }

        public IActionResult EditCustomer(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new
                {
                    result = false,
                    reason = ModelState.GetErrorMessage()
                });
            }
            var result = _customerService.UpdateCustomer(customerViewModel, out var reason);
            return Json(new
            {
                result = result,
                reason = reason
            });
        }

        public void DeleteSingle(int id)
        {
            _customerService.DeleteCustomers(new List<int>(){id});
        }

        public void DeleteMulti(List<int> ids)
        {
            _customerService.DeleteCustomers(ids);
        }
        
        public IActionResult GetAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            return Json(customers);
        }
    }
}