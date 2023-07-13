using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _carService;

        public VehicleController(IVehicleService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VehicleDto>> GetAll()
        {
            return Ok(_carService.GetVehicles());
        }

        [HttpGet("{id}")]
        public ActionResult<VehicleDto> Get([FromRoute] int id)
        {
            var car = _carService.GetVehicle(id);
            if (car is null)
                return NotFound();
            return Ok(car);
        }
    }
}