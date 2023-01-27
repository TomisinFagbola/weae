using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identities;

namespace Application.Mapper
{
    public class WeatherMapper : Profile
    {
        public WeatherMapper()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<WeatherCreateDto, Weather>();
            CreateMap<WeatherUpdateDto, Weather>();

        }
    }
}