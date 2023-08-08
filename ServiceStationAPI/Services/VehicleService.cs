using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceStationAPI.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetVehicles();

        Task<VehicleDto> GetVehicle(int id);

        Task<int> CreateVehicle(CreateVehicleDto dto);

        Task RemoveVehicle(int id);

        Task UpdateVehicle(int id, UpdateVehicleDto dto);
    }

    public class VehicleService : IVehicleService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;
        private readonly IUserContextService _userContextService;

        public VehicleService(ServiceStationDbContext dbContext, IMapper mapper, ILogger<VehicleService> logger, IUserContextService userContextService)

        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<VehicleDto>> GetVehicles()
        {
            var vehicles = await _dbContext.Vehicles.Include(v => v.Owner).Include(v => v.Type).Include(v => v.OrderNotes).ToListAsync();
            if (_userContextService.GetUserRole == "Client")
                vehicles = vehicles.Where(v => v.OwnerId == _userContextService.GetUserId).ToList();
            var vehicleDtos = _mapper.Map<List<VehicleDto>>(vehicles);
            return vehicleDtos;
        }

        public async Task<VehicleDto> GetVehicle(int id)
        {
            var vehicle = await _dbContext.Vehicles.Include(c => c.Owner).Include(v => v.Type).FirstOrDefaultAsync(v => v.Id == id);
            if(_userContextService.GetUserRole == "Client")
                if (vehicle == null || !(vehicle.OwnerId == _userContextService.GetUserId))
                    vehicle = null;
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
            return vehicleDto;
        }

        public async Task<int> CreateVehicle(CreateVehicleDto dto)
        {
            var vehicle = _mapper.Map<CreateVehicleDto, Vehicle>(dto);
            if (_userContextService.GetUserId is null)
                throw new NotFoundException("User not found");
            vehicle.OwnerId = _userContextService.GetUserId.Value;
            await _dbContext.Vehicles.AddAsync(vehicle);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Vehicle with Id:{vehicle.Id} created");
            return vehicle.Id;
        }

        public async Task RemoveVehicle(int id)
        {
            _logger.LogInformation($"Delete action on vehicle with id {id} invoked");
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Vehicle with Id {id} deleted");
        }

        public async Task UpdateVehicle(int id, UpdateVehicleDto dto)
        {
            var vehicle =await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            vehicle.Brand = dto.Brand;
            vehicle.Model = dto.Model;
            vehicle.RegistrationNumber = dto.RegistrationNumber;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Vehicle with Id {id} updated");
        }

        
    }
}