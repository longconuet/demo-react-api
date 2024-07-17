using AutoMapper;
using DemoReactAPI.Dtos;
using DemoReactAPI.Entities;

namespace DemoReactAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
        }
    }
}
