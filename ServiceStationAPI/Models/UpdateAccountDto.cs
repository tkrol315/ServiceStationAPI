using ServiceStationAPI.Entities;

namespace ServiceStationAPI.Models
{
    public class UpdateAccountDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
