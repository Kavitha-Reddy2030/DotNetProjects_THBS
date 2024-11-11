using AutoMapper;
using UserRoleAPI.DataAccessLayer.DTO;
using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.Profiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<UserDTO, User>();

            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ActiveStatus, opt => opt.MapFrom(src => true));

            CreateMap<User, CreateUserDTO>();
        }
    }
}