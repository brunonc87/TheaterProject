using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Rooms;

namespace Theater.Infra.Data.Configuration
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");
            builder.HasKey(r => r.RoomID).HasName("RoomID");
            builder.Property(r => r.Name).HasMaxLength(250).IsRequired().HasColumnName("Name");
            builder.Property(r => r.SeatsNumber).IsRequired().HasColumnName("SeatsNumber");
        }
    }
}
