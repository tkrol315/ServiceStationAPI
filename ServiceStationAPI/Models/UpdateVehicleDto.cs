using System.ComponentModel.DataAnnotations;

namespace ServiceStationAPI.Models
{
    public class UpdateVehicleDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
    }
}