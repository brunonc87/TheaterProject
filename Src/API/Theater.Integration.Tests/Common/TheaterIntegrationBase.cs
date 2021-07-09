using Microsoft.VisualStudio.TestTools.UnitTesting;
using Respawn;
using System.Data.SqlClient;
using Theater.Infra.Data.Common;

namespace Theater.Integration.Tests.Common
{
    [TestClass]
    public class TheaterIntegrationBase
    {
        private static string _theaterConnectionString;
        private static Checkpoint _theaterCheckpoint;
        protected static TheaterContext _theaterContext;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            _theaterCheckpoint = new Checkpoint { WithReseed = true };

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
            //_theaterCheckpoint.Reset(_theaterConnectionString).Wait();
        }
    }
}
