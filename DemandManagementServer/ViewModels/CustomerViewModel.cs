using System.ComponentModel.DataAnnotations;

namespace DemandManagementServer.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "客户名称不能为空")]
        public string Name { get; set; }

        public int CustomerType { get; set; }

        public int CustomerPriority { get; set; }

        public string Remark { get; set; }
    }
}
