using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Domain.Movies;

namespace Theater.Infra.Data.Configuration
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");
            builder.HasKey(m => m.MovieID).HasName("MovieID");
            builder.Property(m => m.Tittle).HasMaxLength(250).IsRequired().HasColumnName("Tittle");
            builder.Property(m => m.Description).HasMaxLength(4000).IsRequired().HasColumnName("Description");
            builder.Property(m => m.Duration).IsRequired().HasColumnName("Duration");
        }
    }
}
