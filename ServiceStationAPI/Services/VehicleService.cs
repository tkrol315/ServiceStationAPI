using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Services
{
    public interface IVehicleService
    {
        IEnumerable<VehicleDto> GetVehicles();

        VehicleDto GetVehicle(int id);
    }

    public class VehicleService : IVehicleService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VehicleService(ServiceStationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<VehicleDto> GetVehicles()
        {
            var cars = _dbContext.Vehicles.Include(c => c.Owner).Include(c => c.Type).ToList();
            var carDtos = _mapper.Map<List<VehicleDto>>(cars);
            return carDtos;
        }

        public VehicleDto GetVehicle(int id)
        {
            var car = _dbContext.Vehicles.Include(c => c.Owner).Include(c => c.Type).FirstOrDefault(c => c.Id == id);
            var carDto = _mapper.Map<VehicleDto>(car);
            return carDto;
        }
    }
}