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
    }
}