using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceStationAPI.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            builder.HasMany(u => u.Cars).WithOne(c => c.Owner).HasForeignKey(c => c.OwnerId);
            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.Surname).IsRequired();
            builder.Property(u => u.Email).IsRequired();
        }
    }
}