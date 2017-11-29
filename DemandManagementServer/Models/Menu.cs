using System.ComponentModel.DataAnnotations;

namespace DemandManagementServer.Models
{
    public class Menu
    {
        public int Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Required]
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

        public override bool Equals(object obj)
        {
            Menu menu = obj as Menu;
            return Id == menu?.Id;
        }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
