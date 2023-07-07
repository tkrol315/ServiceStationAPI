using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceStationAPI.Entities.Configuration
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasOne(c => c.Owner).WithMany(o => o.Cars).HasForeignKey(c => c.OwnerId);
            builder.HasOne(c => c.Type).WithMany(t => t.Cars).HasForeignKey(c => c.TypeId);
            builder.HasMany(c => c.OrderNotes).WithOne(o => o.Car).HasForeignKey(o => o.CarId);
            builder.Property(c => c.Model).IsRequired();
            builder.Property(c => c.Brand).IsRequired();
            builder.Property(c => c.RegistrationNumber).IsRequired();
            builder.Property(c => c.Vin).IsRequired();
        }
    }
}