using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;

namespace ServiceStationAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(m => m.OwnerName, opt => opt.MapFrom(u => u.Owner.Name))
                .ForMember(m => m.OwnerSurname, opt => opt.MapFrom(u => u.Owner.Surname))
                .ForMember(m => m.OwnerEmail, opt => opt.MapFrom(u => u.Owner.Email))
                .ForMember(m => m.VehicleTypeName, opt => opt.MapFrom(c => c.Type.Name));
            CreateMap<OrderNote, OrderNoteDto>()
                .ForMember(m => m.CreatorName, opt => opt.MapFrom(on => on.Creator.Name))
                .ForMember(m => m.CreatorSurname, opt => opt.MapFrom(on => on.Creator.Surname));
            CreateMap<CreateVehicleDto, Vehicle>()
                .ForMember(m => m.Owner, opt => opt.Ignore());
            CreateMap<CreateOrderNoteDto, OrderNote>()
                .ForMember(m => m.Vehicle, opt => opt.Ignore())
                .ForMember(m => m.Creator, opt => opt.Ignore());
           CreateMap<RegisterAccountDto,User>()
                .ForMember(m=>m.Role, opt => opt.Ignore());
        }
    }
}