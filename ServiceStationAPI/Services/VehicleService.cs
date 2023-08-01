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
        private readonly IHttpContextAccessor _contextAccessor;

        public VehicleService(ServiceStationDbContext dbContext, IMapper mapper, ILogger<VehicleService> logger,IHttpContextAccessor contextAccessor)

        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<VehicleDto>> GetVehicles()
        {
            var vehicles = await _dbContext.Vehicles.Include(v => v.Owner).Include(v => v.Type).Include(v=>v.OrderNotes).ToListAsync();
            var vehicleDtos = _mapper.Map<List<VehicleDto>>(vehicles);
            return vehicleDtos;
        }

        public async Task<VehicleDto> GetVehicle(int id)
        {
            var vehicle = await _dbContext.Vehicles.Include(c => c.Owner).Include(v => v.Type).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
            return vehicleDto;
        }

        public async Task<int> CreateVehicle(CreateVehicleDto dto)
        {
            var vehicle = _mapper.Map<CreateVehicleDto, Vehicle>(dto);
            var ownerIdClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (ownerIdClaim != null && Guid.TryParse(ownerIdClaim.Value, out Guid ownerId))
                vehicle.Owner = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == ownerId);
            else
                throw new NotFoundException("Owner not found");
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