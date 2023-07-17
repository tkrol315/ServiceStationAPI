namespace ServiceStationAPI.Models
{
    public class UpdateVehicleDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
    }
}