namespace DemandManagementServer.Models
{
    public class SoftwareVersion
    {
        public int Id { get; set; }

        public string Version { get; set; }

        public string ExpectedStartDate { get; set; }

        public string ExpectedEndDate { get; set; }

        public string ExpectedReleaseDate { get; set; }

        public string ReleaseDate { get; set; }

        public VersionSate VersionSate { get; set; }
    }

    public enum VersionSate
    {
        计划阶段,
        正在实施,
        已发布,
        已废弃
    }
}
