namespace ServiceStationAPI.Entities
{
    public class OrderNote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
        public User Creator { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Created { get; set; }
    }
}