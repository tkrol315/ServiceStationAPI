using Microsoft.EntityFrameworkCore;

namespace ServiceStationAPI.Entities
{
    public class ServiceStationDbContext : DbContext
    {
        public ServiceStationDbContext(DbContextOptions<ServiceStationDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<OrderNote> OrderNotes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}