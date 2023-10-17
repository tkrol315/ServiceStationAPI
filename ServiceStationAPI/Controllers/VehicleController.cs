using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Dtos;
using ServiceStationAPI.Models;
using ServiceStationAPI.Services;
using System.Security.Claims;

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
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehicles()
        {
            return Ok(await _vehicleService.GetVehicles());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle([FromRoute] int id)
        {
            var car = await _vehicleService.GetVehicle(id);
            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicle([FromBody] CreateVehicleDto dto)
        {
            var id = await _vehicleService.CreateVehicle(dto);
            return Created($"api/car/{id}", null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle([FromRoute] int id)
        {
            await _vehicleService.RemoveVehicle(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVehicle([FromRoute] int id, [FromBody] UpdateVehicleDto dto)
        {
            await _vehicleService.UpdateVehicle(id, dto);
            return Ok();
        }
    }
}