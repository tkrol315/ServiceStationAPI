using AutoMapper;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;

namespace ServiceStationAPI
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<Car, CarDto>()
                .ForMember(m => m.OwnerName, on => on.MapFrom(u => u.Owner.Name))
                .ForMember(m => m.OwnerSurname, on => on.MapFrom(u => u.Owner.Surname))
                .ForMember(m => m.OwnerEmail, on => on.MapFrom(u => u.Owner.Email))
                .ForMember(m => m.VehicleTypeName, on => on.MapFrom(c => c.Type.Name));
            CreateMap<OrderNote, OrderNoteDto>()
                .ForMember(m => m.CreatorName, cn => cn.MapFrom(on => on.Creator.Name))
                .ForMember(m => m.CreatorSurname, cn => cn.MapFrom(on => on.Creator.Surname));
        }
    }
}