using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Services
{
    public interface ICarService
    {
        IEnumerable<CarDto> GetCars();
    }

    public class CarService : ICarService
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CarService(ServiceStationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<CarDto> GetCars()
        {
            var cars = _dbContext.Cars.Include(c => c.Owner).Include(c => c.Type).ToList();
            var carDtos = _mapper.Map<List<CarDto>>(cars);
            return carDtos;
        }
    }
}