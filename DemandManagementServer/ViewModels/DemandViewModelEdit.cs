namespace DemandManagementServer.ViewModels
{
    public class DemandViewModelEdit
    {
        public int Id { get; set; }

        public string DemandCode { get; set; }

        public int DemandType { get; set; }

        public string DemandDetail { get; set; }

        public int UserId { get; set; }

        public int CustomerId { get; set; }

        public string Remarks { get; set; }
    }
}
