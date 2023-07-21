using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI
{
    public class Seeder
    {
        private readonly ServiceStationDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public Seeder(ServiceStationDbContext dbContext,IPasswordHasher<User> passwordHasher)

        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.VehicleTypes.Any())
                {
                    var vehicleTypes = GetVehicleTypes();
                    _dbContext.VehicleTypes.AddRange(vehicleTypes);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Vehicles.Any())
                {
                    var vehicle = TestInit();
                    _dbContext.Vehicles.AddRange(vehicle);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>() { new Role() { Name = "Client" }, new Role() { Name = "Mechanic" }, new Role() { Name = "Manager" } };
        }

        private IEnumerable<VehicleType> GetVehicleTypes()
        {
            return new List<VehicleType>() { new VehicleType() { Name = "Car" }, new VehicleType() { Name = "Truck" } };
        }

        private IEnumerable<Vehicle> TestInit()
        {
            var list = new List<Vehicle>()
            {
                new Vehicle(){
                    Brand="Skoda",
                    Model="Octavia",
                    Owner = new User()
                    {
                        Name="Jan",
                        Surname="Kowalski",
                        RoleId = 1,
                        Email ="test@test.com",
                    },
                    TypeId = 1,
                    Vin = "WVGZZZ5NZ8W031284",
                    RegistrationNumber = "SB1234"
                },
                 new Vehicle(){
                    Brand="Passat",
                    Model="B6",
                    Owner = new User()
                    {
                        Name="Adam",
                        Surname="Nowak",
                        RoleId = 1,
                        Email ="test2@test.com",
                    },
                    TypeId = 1,
                    Vin = "WZXZZZ5NZ12W01484",
                    RegistrationNumber = "SB5678"
                }
            };
            list[0].Owner.PasswordHash = _passwordHasher.HashPassword(list[0].Owner, "123456789");
            list[1].Owner.PasswordHash = _passwordHasher.HashPassword(list[1].Owner, "qwertyuiop");
            return list;
        }
    }
}