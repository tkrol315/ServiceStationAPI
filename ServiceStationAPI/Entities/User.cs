﻿namespace ServiceStationAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<OrderNote> OrderNotes { get; set; } = new List<OrderNote>();
    }
}