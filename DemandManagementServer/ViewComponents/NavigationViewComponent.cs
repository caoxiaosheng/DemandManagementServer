using DemandManagementServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        public NavigationViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public IViewComponentResult Invoke()
        {
            var userName = HttpContext.User.Identity.Name;
            var menus = _menuService.GetMenusByUserName(userName);
            return View(menus);
        }
    }
}