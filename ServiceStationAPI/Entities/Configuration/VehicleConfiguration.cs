using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceStationAPI.Entities.Configuration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasOne(c => c.Owner).WithMany(o => o.Vehicles).HasForeignKey(c => c.OwnerId);
            builder.HasOne(c => c.Type).WithMany(t => t.Vehicles).HasForeignKey(c => c.TypeId);
            builder.HasMany(c => c.OrderNotes).WithOne(o => o.Vehicle).HasForeignKey(o => o.CarId);
            builder.Property(c => c.Model).IsRequired();
            builder.Property(c => c.Brand).IsRequired();
            builder.Property(c => c.RegistrationNumber).IsRequired();
            builder.Property(c => c.Vin).IsRequired();
        }
    }
}