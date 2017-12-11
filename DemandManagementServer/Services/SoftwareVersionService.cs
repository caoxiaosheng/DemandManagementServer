using System;
using System.Collections.Generic;
using System.Linq;
using DemandManagementServer.DAL;
using DemandManagementServer.Extensions;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public class SoftwareVersionService:ISoftwareVersionService
    {
        private readonly DemandDbContext _demandDbContext;

        public SoftwareVersionService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }

        public List<SoftwareVersionViewModel> GetSoftwareVersions(int startPage, int pageSize, out int rowCount)
        {
            rowCount = _demandDbContext.SoftwareVersions.Count();
            var softwareVersions = _demandDbContext.SoftwareVersions.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return AutoMapper.Mapper.Map<List<SoftwareVersionViewModel>>(softwareVersions);
        }

        public SoftwareVersionViewModel GetSoftwareVersionById(int id)
        {
            var softwareVersion = _demandDbContext.SoftwareVersions.SingleOrDefault(item => item.Id == id);
            if (softwareVersion.VersionProgress == VersionProgress.已发布)
            {
                return null;
            }
            return AutoMapper.Mapper.Map<SoftwareVersionViewModel>(softwareVersion);
        }

        public bool AddSoftwareVersion(SoftwareVersionViewModel softwareVersionViewModel, out string reason)
        {
            reason = string.Empty;
            var newSoftwareVersion = AutoMapper.Mapper.Map<SoftwareVersion>(softwareVersionViewModel);
            var softwareVersion = _demandDbContext.SoftwareVersions.SingleOrDefault(item => item.VersionName == newSoftwareVersion.VersionName);
            if (softwareVersion != null)
            {
                reason = "已存在名称：" + newSoftwareVersion.VersionName;
                return false;
            }
            newSoftwareVersion.IsDeleted = 0;
            _demandDbContext.SoftwareVersions.Add(newSoftwareVersion);
            _demandDbContext.SaveChanges();
            return true;
        }

        public bool UpdateSoftwareVersion(SoftwareVersionViewModel softwareVersionViewModel, out string reason)
        {
            reason = string.Empty;
            var softwareVersion = _demandDbContext.SoftwareVersions.SingleOrDefault(item => item.Id == softwareVersionViewModel.Id);
            if (softwareVersion == null)
            {
                reason = "未查找到该版本";
                return false;
            }
            if (softwareVersion.VersionProgress == VersionProgress.已发布)
            {
                reason = "已发布版本不允许修改";
                return false;
            }
            //仅名称变了 才需要判断重复
            if (softwareVersionViewModel.VersionName != softwareVersion.VersionName)
            {
                var sameNameVersion = _demandDbContext.SoftwareVersions.SingleOrDefault(item => item.VersionName == softwareVersionViewModel.VersionName);
                if (sameNameVersion != null)
                {
                    reason = "已存在名称：" + sameNameVersion.VersionName;
                    return false;
                }
            }
            var newSoftwareVersion = AutoMapper.Mapper.Map<SoftwareVersion>(softwareVersionViewModel);
            softwareVersion.VersionName = newSoftwareVersion.VersionName;
            softwareVersion.ExpectedStartDate = newSoftwareVersion.ExpectedStartDate;
            softwareVersion.ExpectedEndDate = newSoftwareVersion.ExpectedEndDate;
            softwareVersion.ExpectedReleaseDate = newSoftwareVersion.ExpectedReleaseDate;
            //softwareVersion.ReleaseDate = newSoftwareVersion.ReleaseDate;
            softwareVersion.VersionProgress = newSoftwareVersion.VersionProgress;
            softwareVersion.Remarks = newSoftwareVersion.Remarks;
            _demandDbContext.SaveChanges();
            return true;
        }

        public void DeleteSoftwareVersions(List<int> ids)
        {
            foreach (var id in ids)
            {
                var softwareVersion = _demandDbContext.SoftwareVersions.SingleOrDefault(item => item.Id == id);
                if (softwareVersion != null)
                {
                    softwareVersion.IsDeleted = 1;
                }
            }
            _demandDbContext.SaveChanges();
        }

        public void ReleaseSoftwareVersions(int id)
        {
            var softwareVersion = _demandDbContext.SoftwareVersions.SingleOrDefault(item => item.Id == id);
            if (softwareVersion != null)
            {
                softwareVersion.ReleaseDate=DateTime.Now.Date;
                softwareVersion.VersionProgress = VersionProgress.已发布;
                _demandDbContext.SaveChanges();
            }
        }
    }
}
