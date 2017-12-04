using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemandManagementServer.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "客户名称不能为空")]
        public string Name { get; set; }

        public int CustomerType { get; set; }

        public int CustomerPriority { get; set; }

        [BindNever]
        public int CustomerState { get; set; }

        public string Remarks { get; set; }
    }
}
