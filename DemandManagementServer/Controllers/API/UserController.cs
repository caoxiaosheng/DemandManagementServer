using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers.API
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("api/getallusers")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Json(users);
        }
    }
}
