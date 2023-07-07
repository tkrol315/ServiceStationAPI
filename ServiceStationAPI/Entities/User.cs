namespace ServiceStationAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserType Type { get; set; }
        public int TypeId { get; set; }
        public string Email { get; set; }
        public string? phoneNumber { get; set; }
    }
}