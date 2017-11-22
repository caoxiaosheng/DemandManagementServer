using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemandManagementServer.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "角色名称不能为空。")]
        public string Name { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remarks { get; set; }

        [BindNever]
        public List<RoleMenuViewModel> RoleMenus { get; set; }
    }
}
