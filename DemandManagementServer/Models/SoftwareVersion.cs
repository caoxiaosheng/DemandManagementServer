﻿using System;

namespace DemandManagementServer.Models
{
    public class SoftwareVersion
    {
        public int Id { get; set; }

        public string VersionName { get; set; }

        public DateTime ExpectedStartDate { get; set; }

        public DateTime ExpectedEndDate { get; set; }

        public DateTime ExpectedReleaseDate { get; set; }

        public DateTime ReleaseDate { get; set; }

        public VersionState VersionState { get; set; }

        public int IsDeleted { get; set; }

        public string Remarks { get; set; }
    }

    public enum VersionState
    {
        计划阶段,
        正在实施,
        已发布
    }
}
