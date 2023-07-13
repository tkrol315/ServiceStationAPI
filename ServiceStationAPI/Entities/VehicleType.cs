namespace ServiceStationAPI.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}