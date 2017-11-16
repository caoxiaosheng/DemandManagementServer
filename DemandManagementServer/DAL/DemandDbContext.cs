using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.Models;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DemandManagementServer.DAL
{
    public class DemandDbContext:DbContext
    {
        public DemandDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(userrole => new {userrole.UserId, userrole.RoleId});
            modelBuilder.Entity<UserRole>().HasOne(userrole => userrole.User).WithMany(user => user.UserRoles)
                .HasForeignKey(userrole => userrole.UserId).IsRequired();
            modelBuilder.Entity<UserRole>().HasOne(userrole => userrole.Role).WithMany(role => role.UserRoles)
                .HasForeignKey(userrole => userrole.RoleId).IsRequired();

            modelBuilder.Entity<RoleMenu>().HasKey(rolemenu => new {rolemenu.RoleId, rolemenu.MenuId});
            modelBuilder.Entity<RoleMenu>().HasOne(rolemenu => rolemenu.Role).WithMany(role => role.RoleMenus)
                .HasForeignKey(rolemenu => rolemenu.RoleId).IsRequired();

            modelBuilder.Entity<User>().HasIndex(user => user.UserName).IsUnique();
            modelBuilder.Entity<Menu>().HasIndex(menu => menu.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
