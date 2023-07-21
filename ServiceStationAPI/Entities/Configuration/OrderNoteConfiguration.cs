using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceStationAPI.Entities.Configuration
{
    public class OrderNoteConfiguration : IEntityTypeConfiguration<OrderNote>
    {
        public void Configure(EntityTypeBuilder<OrderNote> builder)
        {
            builder.Property(o => o.Title).IsRequired();
            builder.Property(o => o.Description).IsRequired();
            builder.Property(o => o.Price).HasPrecision(10, 2);
            builder.HasOne(o => o.Creator).WithMany(u => u.OrderNotes).HasForeignKey(o => o.CreatorId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}