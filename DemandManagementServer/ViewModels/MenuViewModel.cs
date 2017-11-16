using System.ComponentModel.DataAnnotations;

namespace DemandManagementServer.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Required(ErrorMessage = "功能名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 功能地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 功能图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 功能备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
