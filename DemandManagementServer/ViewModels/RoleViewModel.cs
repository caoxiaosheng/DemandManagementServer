using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemandManagementServer.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "角色名称不能为空。")]
        public string Name { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remarks { get; set; }

    }
}
