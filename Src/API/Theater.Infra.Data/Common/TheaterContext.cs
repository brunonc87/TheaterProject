using Microsoft.EntityFrameworkCore;
using Theater.Domain.Credentials;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;
using Theater.Infra.Data.Configuration;

namespace Theater.Infra.Data.Common
{
    public class TheaterContext : DbContext
    {
        private readonly string _connectionString;

        public TheaterContext(DbContextOptions<TheaterContext> options) : base(options)
        {
        }

        public TheaterContext(string connectionString)
        {
            //if (string.IsNullOrWhiteSpace(connectionString))
            //    _connectionString = @"Data Source=127.0.0.1\sqlexpress; Initial Catalog=TheaterDatabase; Application Name=Theater; User Id=sa; Password=321;";
            //else 
                _connectionString = connectionString;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_connectionString))
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CredentialConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SectionConfiguration).Assembly);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}
