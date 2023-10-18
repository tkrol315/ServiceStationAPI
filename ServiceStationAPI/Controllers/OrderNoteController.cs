using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/vehicle/{vehicleId}/ordernote")]
    [ApiController]
    [Authorize]
    public class OrderNoteController : ControllerBase
    {
        private readonly IOrderNoteService _orderNoteService;
        public OrderNoteController(IOrderNoteService orderNoteService)
        {
            _orderNoteService = orderNoteService;   
        }
        [HttpPost]
        [Authorize(Roles="Mechanic,Manager")]
        public async Task<ActionResult> CreateOrderNote([FromRoute] int vehicleId, [FromBody] CreateOrderNoteDto dto)
        {
            var noteId =await _orderNoteService.CreateOrderNote(vehicleId, dto);
            return Created($"api/{vehicleId}/ordernote/{noteId}",null);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllOrderNotes([FromRoute]int vehicleId)
        {
            var orderNotes =await _orderNoteService.GetAllOrderNotes(vehicleId);
            return Ok(orderNotes);
        }

        [HttpGet("{orderNoteId}")]
        public async Task<ActionResult> GetOrderNote([FromRoute] int vehicleId, [FromRoute] int orderNoteId)
        {
            var orderNote =await _orderNoteService.GetOrderNoteById(vehicleId, orderNoteId);
            return Ok(orderNote);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{orderNoteId}")]
        public async Task<ActionResult> DeleteOrderNote( [FromRoute] int vehicleId, [FromRoute] int orderNoteId)
        {
            await _orderNoteService.RemoveOrderNoteById(vehicleId, orderNoteId);
            return NoContent();
        }
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteOrderNotes([FromRoute] int vehicleId)
        {
            await _orderNoteService.RemoveOrderNotes(vehicleId);
            return NoContent();
        }
    }
}