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
        public ActionResult Create([FromRoute] int vehicleId, [FromBody] CreateOrderNoteDto dto)
        {
            var noteId = _orderNoteService.CreateOrderNote(vehicleId, dto);
            return Created($"api/{vehicleId}/ordernote/{noteId}",null);
        }
        [HttpGet]
        public ActionResult GetAll([FromRoute]int vehicleId)
        {
            var orderNotes = _orderNoteService.GetOrderNotes(vehicleId);
            return Ok(orderNotes);
        }

        [HttpGet("{orderNoteId}")]
        public ActionResult GetById([FromRoute] int vehicleId, [FromRoute] int orderNoteId)
        {
            var orderNote = _orderNoteService.GetOrderNoteById(vehicleId, orderNoteId);
            return Ok(orderNote);
        }
    }
}