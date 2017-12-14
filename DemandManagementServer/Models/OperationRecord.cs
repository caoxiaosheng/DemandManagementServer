using System;

namespace DemandManagementServer.Models
{
    public class OperationRecord
    {
        public int Id { get; set; }

        public int DemandId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTime DateTime { get; set; }

        public string RecordDetail { get; set; }

        public virtual Demand Demand { get; set; }
    }

    public enum OperationType
    {
        需求对齐,
        需求分析
    }
}
