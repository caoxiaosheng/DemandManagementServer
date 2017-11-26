﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DemandManagementServer.Services
{
    public class UserService:IUserService
    {
        private readonly DemandDbContext _demandDbContext;

        public UserService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }
        
        public User CheckUser(string userName, string password)
        {
            return _demandDbContext.Users.Include(user=>user.UserRoles).ThenInclude(item=>item.Role).FirstOrDefault(it => it.UserName == userName && it.Password == password);
        }

        public List<UserViewModel> GetUsers(int startPage, int pageSize, out int rowCount)
        {
            rowCount = _demandDbContext.Users.Count();
            var users = _demandDbContext.Users.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).Include(item=>item.UserRoles).ThenInclude(item=>item.Role).ToList();
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = AutoMapper.Mapper.Map<UserViewModel>(user);
                string roles = "";
                foreach (var userRole in user.UserRoles)
                {
                    roles += userRole.Role.Name+";";
                }
                userViewModel.Roles = roles;
                userViewModels.Add(userViewModel);
            }
            return userViewModels;
        }
    }
}
