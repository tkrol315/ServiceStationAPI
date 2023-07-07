namespace ServiceStationAPI.Entities
{
    public class CarType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}