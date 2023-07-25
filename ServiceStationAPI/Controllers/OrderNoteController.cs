using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/{vehicleId}/ordernote")]
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
        public ActionResult CreateOrderNote([FromRoute] int vehicleId, [FromBody] CreateOrderNoteDto dto)
        {
            var noteId = _orderNoteService.CreateOrderNote(vehicleId, dto);
            return Created($"api/{vehicleId}/ordernote/{noteId}",null);
        }
        [HttpGet]
        public ActionResult GetAllOrderNotes([FromRoute]int vehicleId)
        {
            var orderNotes = _orderNoteService.GetAllOrderNotes(vehicleId);
            return Ok(orderNotes);
        }

        [HttpGet("{orderNoteId}")]
        public ActionResult GetOrderNote([FromRoute] int vehicleId, [FromRoute] int orderNoteId)
        {
            var orderNote = _orderNoteService.GetOrderNoteById(vehicleId, orderNoteId);
            return Ok(orderNote);
        }
        [HttpDelete("{orderNoteId}")]
        public ActionResult DeleteOrderNote( [FromRoute] int vehicleId, [FromRoute] int orderNoteId)
        {
            _orderNoteService.RemoveOrderNoteById(vehicleId, orderNoteId);
            return NoContent();
        }
        [HttpDelete]
        public ActionResult DeleteOrderNotes([FromRoute] int vehicleId)
        {
            _orderNoteService.RemoveOrderNotes(vehicleId);
            return NoContent();
        }
    }
}