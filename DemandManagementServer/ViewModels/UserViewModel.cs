using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemandManagementServer.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string Remarks { get; set; }
        
        public DateTime LastLoginTime { get; set; }

        public int IsDeleted { get; set; }

        [BindNever]
        public string Roles { get; set; }

        [BindNever]
        public List<UserRoleViewModel> UserRoles { get; set; }
    }
}
