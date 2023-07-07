namespace ServiceStationAPI.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
        public CarType Type { get; set; }
        public int TypeId { get; set; }
        public string Vin { get; set; }
        public string RegistrationNumber { get; set; }
        public List<OrderNote> OrderNotes { get; set; }
    }
}