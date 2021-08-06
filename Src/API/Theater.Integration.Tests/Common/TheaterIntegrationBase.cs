using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Theater.Application.Credentials;
using Theater.Application.Movies;
using Theater.Application.Rooms;
using Theater.Application.Sections;
using Theater.Infra.Data.Common;

namespace Theater.Integration.Tests.Common
{
    [TestClass]
    public class TheaterIntegrationBase
    {
        private static string _theaterConnectionString;
        protected static TheaterContext _theaterContext;
        public static IMapper _mapper;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "TheaterTestIntegration",
                DataSource = "(localdb)\\mssqllocaldb"
            };
            _theaterConnectionString = _builder.ConnectionString;

            _theaterContext = new TheaterContext(_theaterConnectionString);


            try
            {
                _theaterContext.Database.EnsureDeleted();
                _theaterContext.Database.EnsureCreated();
            }
            catch (SqlException) { /*Could not delete, because database was not createad*/ }
        }

        public void Reset()
        {
            SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "TheaterTestIntegration",
                DataSource = "(localdb)\\mssqllocaldb"
            };
            _theaterConnectionString = _builder.ConnectionString;

            _theaterContext = new TheaterContext(_theaterConnectionString);


            try
            {
                _theaterContext.Database.EnsureDeleted();
                _theaterContext.Database.EnsureCreated();
            }
            catch (SqlException) { /*Could not delete, because database was not createad*/ }
        }

        public void ConfigureAutomapper()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CredentialsMappingProfile());
                    mc.AddProfile(new MoviesMappingProfile());
                    mc.AddProfile(new RoomsMappingProfile());
                    mc.AddProfile(new SectionsMappingProfile());
                });
                _mapper = mappingConfig.CreateMapper();
            }
        }
    }
}
