using System;
using System.Linq;
using DemandManagementServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DemandManagementServer.DAL
{
    public class DataInitialization
    {
        public static void Initialize(IApplicationBuilder app)
        {
            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //{
            //    var dbcontext = serviceScope.ServiceProvider.GetService<DemandDbContext>();
            //    if (dbcontext.Users.Any())
            //    {
            //        return;
            //    }
            //    dbcontext.Users.Add(new User()
            //    {
            //        UserName = "admin",
            //        Password = "admin",
            //        CreateTime = DateTime.Now
            //    });
            //    dbcontext.Roles.Add(new Role()
            //    {
            //        Name = "管理员",
            //        CreateTime = DateTime.Now
            //    });
            //    dbcontext.SaveChanges();
            //}
        }
    }
}
