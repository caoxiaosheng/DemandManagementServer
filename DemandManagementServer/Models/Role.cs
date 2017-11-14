using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemandManagementServer.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remarks { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
