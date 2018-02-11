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
                demandViewModel.ReleaseDate= demand.SoftwareVersion?.ReleaseDate;
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
            newDemand.Id = 0;
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
            reason = string.Empty;
            var demand = _demandDbContext.Demands.SingleOrDefault(item => item.Id == demandViewModelEdit.Id);
            if (demand == null)
            {
                reason = "未查找到该需求";
                return false;
            }
            //仅名称变了 才需要判断重复
            if (demandViewModelEdit.DemandCode != demand.DemandCode)
            {
                var sameCodeDemand = _demandDbContext.Demands.SingleOrDefault(item => item.DemandCode == demandViewModelEdit.DemandCode);
                if (sameCodeDemand != null)
                {
                    reason = "已存在需求编号：" + sameCodeDemand.DemandCode;
                    return false;
                }
            }
            demand.DemandCode = demandViewModelEdit.DemandCode;
            demand.DemandType = (DemandType)demandViewModelEdit.DemandType;
            demand.DemandDetail = demandViewModelEdit.DemandDetail;
            demand.UserId = demandViewModelEdit.UserId;
            demand.CustomerId = demandViewModelEdit.CustomerId;
            demand.Remarks = demandViewModelEdit.Remarks;
            _demandDbContext.SaveChanges();
            return true;
        }

        public void DeleteDemand(int id)
        {
            var demand = _demandDbContext.Demands.SingleOrDefault(item => item.Id == id);
            if (demand != null && demand.DemandPhase == DemandPhase.需求提出)
            {
                _demandDbContext.Demands.Remove(demand);
                _demandDbContext.SaveChanges();
            }
        }

        public bool AddDemand(DemandViewModelEditAPI demandViewModelEditApi, out string reason)
        {
            reason = string.Empty;
            var demand = _demandDbContext.Demands.SingleOrDefault(item => item.DemandCode == demandViewModelEditApi.DemandCode);
            if (demand != null)
            {
                reason = "已存在名称：" + demandViewModelEditApi.DemandCode;
                return false;
            }
            var user = _demandDbContext.Users.SingleOrDefault(item => item.UserName == demandViewModelEditApi.User);
            if (user == null)
            {
                reason = "查找不到该提交人";
                return false;
            }
            var customer =
                _demandDbContext.Customers.SingleOrDefault(item => item.Name == demandViewModelEditApi.Customer);
            if (customer == null)
            {
                reason = "查找不到该相关客户";
                return false;
            }
            Demand newDemand = new Demand();
            newDemand.Id = 0;
            newDemand.DemandCode = demandViewModelEditApi.DemandCode;
            newDemand.DemandType = (DemandType)demandViewModelEditApi.DemandType;
            newDemand.DemandDetail = demandViewModelEditApi.DemandDetail;
            newDemand.UserId = user.Id;
            newDemand.CustomerId = customer.Id;
            newDemand.Remarks = demandViewModelEditApi.Remarks;
            newDemand.DemandPhase = DemandPhase.需求提出;
            newDemand.CreateTime = DateTime.Now;
            newDemand.SoftwareVersionId = null;
            _demandDbContext.Demands.Add(newDemand);
            _demandDbContext.SaveChanges();
            return true;
        }
    }
}
