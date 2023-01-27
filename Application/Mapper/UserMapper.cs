using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identities;

namespace Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
           
        }
    }
}