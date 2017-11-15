using System;
using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;
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
            //        CreateTime = DateTime.Now,
            //        UserRoles = new List<UserRole>()
            //        {
            //            new UserRole(){Role =new Role()
            //            {
            //                Name = "管理员",
            //                CreateTime = DateTime.Now
            //            }}
            //        }
            //    });
            //    dbcontext.Roles.First().RoleMenus = new List<RoleMenu>() { new RoleMenu() { Menu = new Menu() { Code = "Menu", Name = "功能管理", Url = "/Menu/Index" } }, new RoleMenu() { Menu = new Menu() { Code = "Role", Name = "角色管理", Url = "/Role/Index" } } };
            //    dbcontext.SaveChanges();
            //}
        }
    }
}
