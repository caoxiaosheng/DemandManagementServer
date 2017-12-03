using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
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
                menus = result,
                rowsCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize)
            });
        }
    }
}