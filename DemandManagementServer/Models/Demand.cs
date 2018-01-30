using System;
using System.Collections.Generic;

namespace DemandManagementServer.Models
{
    public class Demand
    {
        public int Id { get; set; }

        public string DemandCode { get; set; }

        public DemandType DemandType { get; set; }

        public string DemandDetail { get; set; }

        public int UserId { get; set; }

        public int CustomerId { get; set; }

        public int SoftwareVersionId { get; set; }

        public DemandPhase DemandPhase { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remarks { get; set; }

        public virtual User User { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual List<OperationRecord> OperationRecords { get; set; }

        public virtual SoftwareVersion SoftwareVersion { get; set; }
    }

    public enum DemandType
    {
        网管,
        系统
    }

    public enum DemandPhase
    {
        需求提出,
        需求分析,
        版本计划,
        功能开发,
        等待反馈,
        打回,
        中止,
        完成
    }
}
