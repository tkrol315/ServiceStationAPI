using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Models;

namespace ServiceStationAPI.Services
{
    public interface IVehicleService
    {
        IEnumerable<VehicleDto> GetVehicles();

        VehicleDto GetVehicle(int id);

        int CreateVehicle(CreateVehicleDto dto);

        void DeleteVehicle(int id);

        void UpdateVehicle(int id, UpdateVehicleDto dto);
    }

    public class VehicleService : IVehicleService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(ServiceStationDbContext dbContext, IMapper mapper, ILogger<VehicleService> logger)

        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<VehicleDto> GetVehicles()
        {
            var vehicles = _dbContext.Vehicles.Include(v => v.Owner).Include(v => v.Type).ToList();
            var vehicleDtos = _mapper.Map<List<VehicleDto>>(vehicles);
            return vehicleDtos;
        }

        public VehicleDto GetVehicle(int id)
        {
            var vehicle = _dbContext.Vehicles.Include(c => c.Owner).Include(v => v.Type).FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
            return vehicleDto;
        }

        public int CreateVehicle(CreateVehicleDto dto)
        {
            var vehicle = _mapper.Map<CreateVehicleDto, Vehicle>(dto);
            vehicle.Owner = GetOrCreateOwner(dto);
            _dbContext.Vehicles.Add(vehicle);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Vehicle with Id:{vehicle.Id} created");
            return vehicle.Id;
        }

        public void DeleteVehicle(int id)
        {
            _logger.LogWarning($"Delete action on vehicle with id {id} invoked");
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            _dbContext.Vehicles.Remove(vehicle);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Vehicle with Id {id} deleted");
        }

        public void UpdateVehicle(int id, UpdateVehicleDto dto)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            vehicle.Brand = dto.Brand;
            vehicle.Model = dto.Model;
            vehicle.RegistrationNumber = dto.RegistrationNumber;
            _dbContext.SaveChanges();
            _logger.LogInformation($"Vehicle with Id {id} updated");
        }

        private User GetOrCreateOwner(CreateVehicleDto dto)
        {
            var owner = _dbContext.Users.FirstOrDefault(u =>
                u.Name.ToLower() == dto.OwnerName.ToLower() &&
                u.Surname.ToLower() == dto.OwnerSurname.ToLower() &&
                u.Email.ToLower() == dto.Email.ToLower());

            if (owner is null)
            {
                owner = new User()
                {
                    Name = dto.OwnerName,
                    Surname = dto.OwnerSurname,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    PasswordHash = "temporary - solution",
                    RoleId = 1
                };
            }

            return owner;
        }
    }
}