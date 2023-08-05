using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceStationAPI.Entities;
using System.Runtime.CompilerServices;

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

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                   await _dbContext.Database.MigrateAsync();
                }
                if (!await _dbContext.Roles.AnyAsync())
                {
                    var roles = GetRoles();
                    await _dbContext.Roles.AddRangeAsync(roles);
                    await _dbContext.SaveChangesAsync();
                }
                if (!await _dbContext.VehicleTypes.AnyAsync())
                {
                    var vehicleTypes = GetVehicleTypes();
                    await _dbContext.VehicleTypes.AddRangeAsync(vehicleTypes);
                    await _dbContext.SaveChangesAsync();
                }
                if (!await _dbContext.Users.AnyAsync())
                {
                    var users = GetUsers();
                    await _dbContext.Users.AddRangeAsync(users);
                    await _dbContext.SaveChangesAsync();
                }
                if (!await _dbContext.Vehicles.AnyAsync())
                {
                    var vehicle = GetVhiclesWithOwners();
                    await _dbContext.Vehicles.AddRangeAsync(vehicle);
                    await _dbContext.SaveChangesAsync();
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

        private IEnumerable<Vehicle> GetVhiclesWithOwners()
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
        private IEnumerable<User> GetUsers()
        {
            var list = new List<User>()
            {
                new User()
                {
                     Name="Marek",
                     Surname="Nowak",
                     RoleId = 2,
                     Email ="test1@mechanic.com",
                },
                new User()
                {
                     Name="Ryszard",
                     Surname="Kowalski",
                     RoleId = 2,
                     Email ="test2@mechanic.com",
                },
                 new User()
                {
                     Name="Piotrek",
                     Surname="Krol",
                     RoleId = 3,
                     Email ="manager@manager.com",
                }
            };
            list[0].PasswordHash = _passwordHasher.HashPassword(list[0], "mechanic1123");
            list[1].PasswordHash = _passwordHasher.HashPassword(list[1], "mechanic2123");
            list[2].PasswordHash = _passwordHasher.HashPassword(list[2], "manager123");
            return list;
        }
    }
}