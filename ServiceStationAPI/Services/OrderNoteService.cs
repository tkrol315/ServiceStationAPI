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
        int CreateOrderNote(int id, CreateOrderNoteDto dto);
        IEnumerable<OrderNoteDto> GetAllOrderNotes(int vehicleId);
        OrderNoteDto GetOrderNoteById(int vehicleId, int orderNoteId);
        void RemoveOrderNoteById(int vehicleId, int orderNoteId);
        void RemoveOrderNotes(int vehicleId);

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
        private Vehicle GetVehicle(int vehicleId)
        {
            var vehicle = _dbContext.Vehicles.Include(v => v.OrderNotes).FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            return vehicle;
        }
        public int CreateOrderNote(int id, CreateOrderNoteDto dto)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var orderNote = _mapper.Map<OrderNote>(dto);
            orderNote.Vehicle = vehicle;
            var creatorIdClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (creatorIdClaim != null && Guid.TryParse(creatorIdClaim.Value, out Guid creatorId))
                orderNote.Creator = _dbContext.Users.FirstOrDefault(u => u.Id == creatorId);
            else
                throw new NotFoundException("Creator not found");
            _dbContext.OrderNotes.Add(orderNote);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Order note with Id:{orderNote.Id} created");
            return orderNote.Id;
        }

        public IEnumerable<OrderNoteDto> GetAllOrderNotes(int vehicleId)
        {
            var orderNotes = _dbContext.OrderNotes.Include(o=>o.Creator).Where(o => o.VehicleId == vehicleId).ToList();
            if(orderNotes == null)
                throw new NotFoundException("Vehicle not found");
            var orderNoteDtos = _mapper.Map<List<OrderNoteDto>>(orderNotes);
            return orderNoteDtos;
        }
        public OrderNoteDto GetOrderNoteById(int vehicleId,int orderNoteId)
        {
            var vehicle = _dbContext.Vehicles.Include(v=>v.OrderNotes).ThenInclude(o=>o.Creator).FirstOrDefault(v=>v.Id ==  vehicleId);
            if(vehicle == null)
                throw new NotFoundException("Vehicle not found");
            var orderNote = vehicle.OrderNotes.FirstOrDefault(n=>n.Id == orderNoteId);
            if(orderNote == null)
                throw new NotFoundException("Order note not found");
            var orderNoteDto = _mapper.Map<OrderNoteDto>(orderNote);
            return orderNoteDto;
        }

        public void RemoveOrderNoteById(int vehicleId, int orderNoteId)
        {
            _logger.LogInformation($"Delete action on order note with id {orderNoteId} invoked");
            var vehicle = GetVehicle(vehicleId);
            var orderNote = vehicle.OrderNotes.FirstOrDefault(o => o.Id == orderNoteId);
            if (orderNote == null)
                throw new NotFoundException("Order note not found");
            vehicle.OrderNotes.Remove(orderNote);
            _dbContext.SaveChanges();
            _logger.LogInformation($"order note with Id {orderNoteId} deleted");
        }
        public void RemoveOrderNotes(int vehicleId)
        {
            _logger.LogInformation($"Delete action on all order notes from vehicle with id {vehicleId} invoked");
            var vehicle = GetVehicle(vehicleId);
            vehicle.OrderNotes.RemoveRange(0, vehicle.OrderNotes.Count);
            _dbContext.SaveChanges();
            _logger.LogInformation($"all order notes from vehicle with id {vehicleId} deleted");
        }
    }
}
