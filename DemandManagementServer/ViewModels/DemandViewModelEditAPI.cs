using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemandManagementServer.ViewModels
{
    public class DemandViewModelEditAPI
    {
        public int Id { get; set; }

        public string DemandCode { get; set; }

        public int DemandType { get; set; }

        public string DemandDetail { get; set; }

        public string User { get; set; }

        public string Customer { get; set; }

        public string Remarks { get; set; }
    }
}
