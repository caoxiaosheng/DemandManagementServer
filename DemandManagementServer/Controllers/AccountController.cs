using System.Security.Claims;
using System.Threading.Tasks;
using DemandManagementServer.Services;
using DemandManagementServer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.CheckUser(loginViewModel.UserName, loginViewModel.Password);
                if (user != null)
                {
                    var claimIdentity = new ClaimsIdentity("Cookie");
                    claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claimIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    //claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    //claimIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.MobileNumber));
                    foreach (var role in user.UserRoles)
                    {
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));
                    }
                    var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Index", "Management");
                }
                ViewBag.ErrorInfo = "用户名或密码错误";
                return View();
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}