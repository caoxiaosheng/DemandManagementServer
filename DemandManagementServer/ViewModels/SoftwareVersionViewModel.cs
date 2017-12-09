using System;
using System.ComponentModel.DataAnnotations;

namespace DemandManagementServer.ViewModels
{
    public class SoftwareVersionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "版本名称不能为空")]
        public string VersionName { get; set; }

        public DateTime ExpectedStartDate { get; set; }

        public DateTime ExpectedEndDate { get; set; }

        public DateTime ExpectedReleaseDate { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int VersionProgress { get; set; }

        public int IsDeleted { get; set; }

        public string Remarks { get; set; }
    }
}
