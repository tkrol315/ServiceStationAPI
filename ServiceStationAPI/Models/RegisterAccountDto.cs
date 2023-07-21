using ServiceStationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceStationAPI.Models
{
    public class RegisterAccountDto
    {
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<OrderNote> OrderNotes { get; set; } = new List<OrderNote>();
        public int RoleId { get; set; } = 1;
    }
}
