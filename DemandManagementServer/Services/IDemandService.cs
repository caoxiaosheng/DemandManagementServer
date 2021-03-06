﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface IDemandService
    {
        List<DemandViewModel> GetDemands(int startPage, int pageSize, out int rowCount);
        DemandViewModelEdit GetDemandById(int id);
        bool AddDemand(DemandViewModelEdit demandViewModelEdit, out string reason);
        bool UpdateDemand(DemandViewModelEdit demandViewModelEdit, out string reason);
        void DeleteDemand(int id);
        bool AddDemand(DemandViewModelEditAPI demandViewModelEditApi, out string reason);

        List<DemandViewModel> GetActiveDemands(string userName);
        List<DemandViewModel> GetEndDemands(string userName);
    }
}
