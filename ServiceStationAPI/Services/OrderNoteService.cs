using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Models;
using System.Security.Claims;

namespace ServiceStationAPI.Services
{
    public interface IOrderNoteService
    {
        Task<int> CreateOrderNote(int id, CreateOrderNoteDto dto);
        Task<IEnumerable<OrderNoteDto>> GetAllOrderNotes(int vehicleId);
        Task<OrderNoteDto> GetOrderNoteById(int vehicleId, int orderNoteId);
        Task RemoveOrderNoteById(int vehicleId, int orderNoteId);
        Task RemoveOrderNotes(int vehicleId);

    }
    public class OrderNoteService:IOrderNoteService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<OrderNoteService> _logger;
        public OrderNoteService(ServiceStationDbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor, ILogger<OrderNoteService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _logger = logger;

        }
        private async Task<Vehicle> GetVehicle(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.Include(v => v.OrderNotes).FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            return vehicle;
        }
        public async Task<int> CreateOrderNote(int id, CreateOrderNoteDto dto)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var orderNote = _mapper.Map<OrderNote>(dto);
            orderNote.Vehicle = vehicle;
            var creatorIdClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (creatorIdClaim != null && Guid.TryParse(creatorIdClaim.Value, out Guid creatorId))
                orderNote.Creator =await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == creatorId);
            else
                throw new NotFoundException("Creator not found");
            await _dbContext.OrderNotes.AddAsync(orderNote);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Order note with Id:{orderNote.Id} created");
            return orderNote.Id;
        }

        public async Task<IEnumerable<OrderNoteDto>> GetAllOrderNotes(int vehicleId)
        {
            var orderNotes = await _dbContext.OrderNotes.Include(o=>o.Creator).Where(o => o.VehicleId == vehicleId).ToListAsync();
            if(orderNotes == null)
                throw new NotFoundException("Vehicle not found");
            var orderNoteDtos = _mapper.Map<List<OrderNoteDto>>(orderNotes);
            return orderNoteDtos;
        }
        public async Task<OrderNoteDto> GetOrderNoteById(int vehicleId,int orderNoteId)
        {
            var vehicle = await _dbContext.Vehicles.Include(v => v.OrderNotes).ThenInclude(o => o.Creator).FirstOrDefaultAsync(v => v.Id == vehicleId);
            if(vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var orderNote = vehicle.OrderNotes.FirstOrDefault(n=>n.Id == orderNoteId);
            if(orderNote == null)
                throw new NotFoundException("Order note not found");
            var orderNoteDto = _mapper.Map<OrderNoteDto>(orderNote);
            return orderNoteDto;
        }

        public async Task RemoveOrderNoteById(int vehicleId, int orderNoteId)
        {
            _logger.LogInformation($"Delete action on order note with id {orderNoteId} invoked");
            var vehicle = await GetVehicle(vehicleId);
            var orderNote = vehicle.OrderNotes.FirstOrDefault(o => o.Id == orderNoteId);
            if (orderNote == null)
                throw new NotFoundException("Order note not found");
            vehicle.OrderNotes.Remove(orderNote);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"order note with Id {orderNoteId} deleted");
        }
        public async Task RemoveOrderNotes(int vehicleId)
        {
            _logger.LogInformation($"Delete action on all order notes from vehicle with id {vehicleId} invoked");
            var vehicle = await GetVehicle(vehicleId);
            vehicle.OrderNotes.RemoveRange(0, vehicle.OrderNotes.Count);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"all order notes from vehicle with id {vehicleId} deleted");
        }
    }
}
