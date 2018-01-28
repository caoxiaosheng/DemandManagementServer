using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.ViewModels;

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
            var demands = _demandDbContext.Demands.OrderBy(item => item.Id).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return AutoMapper.Mapper.Map<List<DemandViewModel>>(demands);
        }
    }
}
