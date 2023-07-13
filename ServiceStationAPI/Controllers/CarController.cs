using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CarDto>> GetAll()
        {
            return Ok(_carService.GetCars());
        }
    }
}