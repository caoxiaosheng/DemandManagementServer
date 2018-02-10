using System.Linq;
using DemandManagementServer.DAL;
using DemandManagementServer.Models;
using Microsoft.EntityFrameworkCore;

namespace DemandManagementServer.Services
{
    public class WxUserService: IWxUserService
    {
        private readonly DemandDbContext _demandDbContext;

        public WxUserService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }

        public bool CheckOpenId(string openId, out WxUser wxUser)
        {
            wxUser = _demandDbContext.WxUsers.Include(item => item.User)
                .FirstOrDefault(item => item.OpenId == openId);
            return wxUser != null;
        }

        public bool CheckUserId(int userId)
        {
            var wxUser = _demandDbContext.WxUsers.FirstOrDefault(item => item.UserId == userId);
            return wxUser == null;
        }

        public void AddWxUser(WxUser wxUser)
        {
            _demandDbContext.WxUsers.Add(wxUser);
            _demandDbContext.SaveChanges();
        }
    }
}
