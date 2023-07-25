using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Models;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VehicleDto>> GetVehicles()
        {
            return Ok(_vehicleService.GetVehicles());
        }

        [HttpGet("{id}")]
        public ActionResult<VehicleDto> GetVehicle([FromRoute] int id)
        {
            var car = _vehicleService.GetVehicle(id);
            return Ok(car);
        }

        [HttpPost]
        public ActionResult CreateVehicle([FromBody] CreateVehicleDto dto)
        {
            var id = _vehicleService.CreateVehicle(dto);
            return Created($"api/car/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteVehicle([FromRoute] int id)
        {
            _vehicleService.RemoveVehicle(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateVehicle([FromRoute] int id, [FromBody] UpdateVehicleDto dto)
        {
            _vehicleService.UpdateVehicle(id, dto);
            return Ok();
        }
    }
}