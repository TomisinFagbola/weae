using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identities;

namespace Application.Mapper
{
    public class StateMapper : Profile
    {
        public StateMapper()
        {
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<StateCreateDto, State>();
            CreateMap<StateUpdateDto, State>();

        }
    }
}