using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Models;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VehicleDto>> GetAll()
        {
            return Ok(_vehicleService.GetVehicles());
        }

        [HttpGet("{id}")]
        public ActionResult<VehicleDto> Get([FromRoute] int id)
        {
            var car = _vehicleService.GetVehicle(id);
            return Ok(car);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateVehicleDto dto)
        {
            var id = _vehicleService.CreateVehicle(dto);
            return Created($"api/car/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _vehicleService.DeleteVehicle(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateVehicleDto dto)
        {
            _vehicleService.UpdateVehicle(id, dto);
            return Ok();
        }
    }
}