using DemandManagementServer.Models;

namespace DemandManagementServer.Services
{
    public interface IWxUserService
    {
        bool CheckOpenId(string openId, out WxUser wxUser);
        bool CheckUserId(int userId);
        void AddWxUser(WxUser wxUser);
    }
}
