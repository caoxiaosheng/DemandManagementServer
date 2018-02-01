using System;
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

        public UserViewModel GetUserById(int id)
        {
            var user = _demandDbContext.Users.Include(item => item.UserRoles).FirstOrDefault(item => item.Id == id);
            return AutoMapper.Mapper.Map<UserViewModel>(user);
        }

        public bool AddUser(UserViewModel userViewModel, out string reason)
        {
            reason = string.Empty;
            var newUser = AutoMapper.Mapper.Map<User>(userViewModel);
            var user = _demandDbContext.Users.FirstOrDefault(item => item.UserName == newUser.UserName);
            if (user != null)
            {
                reason = "已存在名称：" + newUser.UserName;
                return false;
            }
            newUser.Password = "123456";
            newUser.CreateTime = DateTime.Now;
            newUser.IsDeleted = 0;
            _demandDbContext.Users.Add(newUser);
            _demandDbContext.SaveChanges();
            return true;
        }

        public bool UpdateUser(UserViewModel userViewModel, out string reason)
        {
            reason = string.Empty;
            if (userViewModel.Id == 1)
            {
                reason = "admin账号不允许修改";
                return false;
            }
            var user = _demandDbContext.Users.Include(item => item.UserRoles)
                .FirstOrDefault(item => item.Id == userViewModel.Id);
            if (user == null)
            {
                reason = "未找到该用户";
                return false;
            }
            //仅名称变了 才需要判断重复
            if (userViewModel.UserName != user.UserName)
            {
                var sameUserNameUser = _demandDbContext.Users.FirstOrDefault(item => item.UserName == userViewModel.UserName);
                if (sameUserNameUser != null)
                {
                    reason = "已存在名称：" + userViewModel.Name;
                    return false;
                }
            }
            var newUser = AutoMapper.Mapper.Map<User>(userViewModel);
            user.UserRoles.Clear();
            _demandDbContext.SaveChanges();
            user.UserRoles = newUser.UserRoles;
            user.UserName = newUser.UserName;
            user.Name = newUser.Name;
            user.Email = newUser.Email;
            user.MobileNumber = newUser.MobileNumber;
            user.Remarks = newUser.Remarks;
            user.CreateTime=DateTime.Now;
            _demandDbContext.SaveChanges();
            return true;
        }

        public void DeleteUser(int id)
        {
            if (id == 1)
            {
                return;
            }
            var user = _demandDbContext.Users.Include(item => item.UserRoles).SingleOrDefault(item => item.Id == id);
            if (user == null)
            {
                return;
            }
            user.IsDeleted = 1;
            _demandDbContext.SaveChanges();
        }

        public void DeleteUsers(List<int> ids)
        {
            foreach (var id in ids)
            {
                if (id == 1)
                {
                    continue;
                }
                var user = _demandDbContext.Users.Include(item => item.UserRoles).SingleOrDefault(item => item.Id == id);
                if (user != null)
                {
                    user.IsDeleted = 1;
                }
            }
            _demandDbContext.SaveChanges();
        }

        public List<UserViewModel> GetAllUsers()
        {
            var users = _demandDbContext.Users.OrderBy(item => item.Id);
            return AutoMapper.Mapper.Map<List<UserViewModel>>(users);
        }

        public UserViewModel GetUserByUserName(string userName)
        {
            var user = _demandDbContext.Users.Include(item => item.UserRoles).FirstOrDefault(item => item.UserName == userName);
            return AutoMapper.Mapper.Map<UserViewModel>(user);
        }
    }
}
