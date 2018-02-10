using System.ComponentModel.DataAnnotations;

namespace DemandManagementServer.Models
{
    public class WxUser
    {
        [Key]
        public string OpenId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
