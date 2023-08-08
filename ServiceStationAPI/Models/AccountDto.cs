using ServiceStationAPI.Dtos;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Models
{
    public class AccountDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
        public List<OrderNoteDto> OrderNotes { get; set; }
    }
}
