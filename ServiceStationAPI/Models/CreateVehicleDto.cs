using System.ComponentModel.DataAnnotations;

namespace ServiceStationAPI.Models
{
    public class CreateVehicleDto
    {
       
        public string Brand { get; set; }
        public string Model { get; set; }
        public int TypeId { get; set; }
        public string Vin { get; set; }
        public string RegistrationNumber { get; set; }
    }
}