using Microsoft.AspNetCore.Mvc;

namespace DemandManagementServer.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public string Invoke()
        {
            return "NavigationViewComponent";
        }
    }
}