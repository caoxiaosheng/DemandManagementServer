using System;
using Newtonsoft.Json;

namespace DemandManagementServer.ViewModels
{
    public class DemandViewModel
    {
        public int Id { get; set; }

        public string DemandCode { get; set; }

        public string DemandType { get; set; }

        public string DemandDetail { get; set; }

        public string User { get; set; }

        public string Customer { get; set; }

        [JsonConverter(typeof(MyDateTimeConverter))]
        public DateTime? CreateTime { get; set; }

        public string DemandPhase { get; set; }

        public string AlignRecords { get; set; }

        public string AnalyseRecords { get; set; }

        public string SoftwareVersion { get; set; }

        [JsonConverter(typeof(MyDateConverter))]
        public DateTime? ReleaseDate { get; set; }

        public string Remarks { get; set; }
    }
}
