using System.Collections.Generic;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.Services
{
    public interface ISoftwareVersionService
    {
        List<SoftwareVersionViewModel> GetSoftwareVersions(int startPage, int pageSize, out int rowCount);
        SoftwareVersionViewModel GetSoftwareVersionById(int id);
        bool AddSoftwareVersion(SoftwareVersionViewModel softwareVersionViewModel, out string reason);
        bool UpdateSoftwareVersion(SoftwareVersionViewModel softwareVersionViewModel, out string reason);
        void DeleteSoftwareVersions(List<int> ids);
        void ReleaseSoftwareVersions(int id);
    }
}
