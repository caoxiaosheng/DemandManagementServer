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
                cfg.CreateMap<Role, RoleViewModel>();
                cfg.CreateMap<RoleViewModel, Role>();
                cfg.CreateMap<RoleMenu, RoleMenuViewModel>();
                cfg.CreateMap<RoleMenuViewModel, RoleMenu>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserViewModel, User>();
            });
        }
    }
}
