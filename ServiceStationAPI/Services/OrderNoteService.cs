using AutoMapper;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Models;

namespace ServiceStationAPI.Services
{
    public interface IOrderNoteService
    {
        OrderNote CreateOrderNote(int id, CreateOrderNoteDto dto);
    }
    public class OrderNoteService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;
        public OrderNoteService(ServiceStationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }
        //public OrderNote CreateOrderNote(int id, CreateOrderNoteDto dto)
        //{
        //    var vehicle = _dbContext.Vehicles.FirstOrDefault(v=>v.Id == id);
        //    if (vehicle == null)
        //        throw new NotFoundException("Vehicle not found");
        //    var orderNote = _mapper.Map<OrderNote>(dto);


        //}

        
    }
}
