using DemandManagementServer.Models;
using DemandManagementServer.ViewModels;

namespace DemandManagementServer.DAL
{
    public class MapperInitialization
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Menu, MenuViewModel>();
                cfg.CreateMap<MenuViewModel, Menu>();
            });
        }
    }
}
