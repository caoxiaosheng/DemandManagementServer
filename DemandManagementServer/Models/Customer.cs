using System;

namespace DemandManagementServer.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CustomerType CustomerType { get; set; }

        public CustomerPriority CustomerPriority { get; set; }

        public CustomerState CustomerState { get; set; }

        public string Remarks { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public enum CustomerType
    {
        免费用户,
        付费用户
    }

    public enum CustomerPriority
    {
        低,
        正常,
        高
    }

    public enum CustomerState
    {
        正常,
        封禁
    }
}
