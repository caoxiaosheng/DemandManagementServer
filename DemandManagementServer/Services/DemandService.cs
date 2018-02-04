using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DemandManagementServer.Services
{
    public class DemandService:IDemandService
    {
        private readonly DemandDbContext _demandDbContext;

        public DemandService(DemandDbContext demandDbContext)
        {
            _demandDbContext = demandDbContext;
        }

        public List<DemandViewModel> GetDemands(int startPage, int pageSize, out int rowCount)
        {
            rowCount = _demandDbContext.Demands.Count();
            var demands = _demandDbContext.Demands.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize)
                .Take(pageSize).Include(item => item.User).Include(item => item.Customer)
                .Include(item => item.SoftwareVersion).Include(item => item.OperationRecords).ToList();
            List<DemandViewModel> demandViewModels=new List<DemandViewModel>();
            foreach (var demand in demands)
            {
                DemandViewModel demandViewModel=new DemandViewModel();
                demandViewModel.Id = demand.Id;
                demandViewModel.DemandCode = demand.DemandCode;
                demandViewModel.DemandType = demand.DemandType.ToString();
                demandViewModel.DemandDetail = demand.DemandDetail;
                demandViewModel.User = demand.User.UserName;
                demandViewModel.Customer = demand.Customer.Name;
                demandViewModel.CreateTime = demand.CreateTime;
                demandViewModel.DemandPhase = demand.DemandPhase.ToString();
                var alignRecords = demand.OperationRecords.Where(item => item.OperationType == OperationType.需求对齐).OrderBy(item=>item.DateTime);
                var alignRecordsString = "";
                foreach (var record in alignRecords)
                {
                    alignRecordsString = alignRecordsString + record.DateTime + record.RecordDetail+"\n";
                }
                alignRecordsString.TrimEnd('\n');
                demandViewModel.AlignRecords = alignRecordsString;
                var analyseRecords = demand.OperationRecords.Where(item => item.OperationType == OperationType.需求分析).OrderBy(item => item.DateTime);
                var analyseRecordsString = "";
                foreach (var record in analyseRecords)
                {
                    analyseRecordsString = analyseRecordsString + record.DateTime + record.RecordDetail + "\n";
                }
                analyseRecordsString.TrimEnd('\n');
                demandViewModel.AnalyseRecords = analyseRecordsString;
                demandViewModel.SoftwareVersion = demand.SoftwareVersion==null?"": demand.SoftwareVersion.VersionName;
                demandViewModel.ReleaseDate= demand.SoftwareVersion == null ?"": (demand.SoftwareVersion.ReleaseDate==null?"": demand.SoftwareVersion.ReleaseDate.ToString());
                demandViewModel.Remarks = demand.Remarks;
                demandViewModels.Add(demandViewModel);
            }
            return demandViewModels;
        }

        public DemandViewModelEdit GetDemandById(int id)
        {
            var demand = _demandDbContext.Demands.SingleOrDefault(item => item.Id == id);
            return AutoMapper.Mapper.Map<DemandViewModelEdit>(demand);
        }

        public bool AddDemand(DemandViewModelEdit demandViewModelEdit, out string reason)
        {
            reason = string.Empty;
            var demand = _demandDbContext.Demands.SingleOrDefault(item => item.DemandCode == demandViewModelEdit.DemandCode);
            if (demand != null)
            {
                reason = "已存在名称：" + demandViewModelEdit.DemandCode;
                return false;
            }
            Demand newDemand=new Demand();
            newDemand.Id = demandViewModelEdit.Id;
            newDemand.DemandCode = demandViewModelEdit.DemandCode;
            newDemand.DemandType = (DemandType) demandViewModelEdit.DemandType;
            newDemand.DemandDetail = demandViewModelEdit.DemandDetail;
            newDemand.UserId = demandViewModelEdit.UserId;
            newDemand.CustomerId = demandViewModelEdit.CustomerId;
            newDemand.Remarks = demandViewModelEdit.Remarks;

            newDemand.DemandPhase = DemandPhase.需求提出;
            newDemand.CreateTime=DateTime.Now;
            newDemand.SoftwareVersionId = null;
            _demandDbContext.Demands.Add(newDemand);
            _demandDbContext.SaveChanges();
            return true;
        }

        public bool UpdateDemand(DemandViewModelEdit demandViewModelEdit, out string reason)
        {
            throw new NotImplementedException();
        }

        public void DropDemands(List<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}
