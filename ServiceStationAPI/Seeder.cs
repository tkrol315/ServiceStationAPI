using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceStationAPI.Entities;

namespace ServiceStationAPI
{
    public class Seeder
    {
        private readonly ServiceStationDbContext _dbContext;

        public Seeder(ServiceStationDbContext dbContext)

        {
            _dbContext = dbContext;
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
                if (!_dbContext.Cars.Any())
                {
                    var vehicle = TestInit();
                    _dbContext.Cars.AddRange(vehicle);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>() { new Role() { Name = "Client" }, new Role() { Name = "Manager" } };
        }

        private IEnumerable<VehicleType> GetVehicleTypes()
        {
            return new List<VehicleType>() { new VehicleType() { Name = "Car" }, new VehicleType() { Name = "Truck" } };
        }

        private IEnumerable<Car> TestInit()
        {
            return new List<Car>()
            {
                new Car(){
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
                 new Car(){
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
        }
    }
}