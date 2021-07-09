using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Sections;

namespace Theater.Infra.Data.Configuration
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Sections");
            builder.HasKey(s => s.SectionID).HasName("SectionID");
            builder.Property(s => s.StartDate).IsRequired().HasColumnName("StartDate");
            builder.Ignore(s => s.FinishDate);
            builder.Property(s => s.Value).IsRequired().HasColumnName("Value");
            builder.Property(s => s.AnimationType).IsRequired().HasColumnName("AnimationType");
            builder.Property(s => s.AudioType).IsRequired().HasColumnName("AudioType");

            builder.HasOne(s => s.Movie).WithMany(m => m.Sections).HasForeignKey(s => s.MovieID);
            builder.Property(s => s.MovieID).HasColumnName("MovieID");

            builder.HasOne(s => s.Room).WithMany(r => r.Sections).HasForeignKey(s => s.RoomID);
            builder.Property(s => s.RoomID).HasColumnName("RoomID");
        }
    }
}
