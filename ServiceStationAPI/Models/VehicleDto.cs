using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;

namespace ServiceStationAPI.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurname { get; set; }
        public string OwnerEmail { get; set; }
        public string VehicleTypeName { get; set; }
        public List<OrderNoteDto> OrderNotes { get; set; }
    }
}