using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DemandManagementServer.ViewModels
{
    public class SoftwareVersionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "版本名称不能为空")]
        public string VersionName { get; set; }

        [JsonConverter(typeof(MyDateConverter))]
        [Required(ErrorMessage = "预计开始时间（开发）不能为空")]
        public DateTime? ExpectedStartDate { get; set; }

        [JsonConverter(typeof(MyDateConverter))]
        [Required(ErrorMessage = "预计结束时间（开发）不能为空")]
        public DateTime? ExpectedEndDate { get; set; }

        [JsonConverter(typeof(MyDateConverter))]
        [Required(ErrorMessage = "预计发布时间不能为空")]
        public DateTime? ExpectedReleaseDate { get; set; }

        [BindNever]
        [JsonConverter(typeof(MyDateConverter))]
        public DateTime? ReleaseDate { get; set; }

        public int VersionProgress { get; set; }

        [BindNever]
        public int IsDeleted { get; set; }

        public string Remarks { get; set; }
    }
}
