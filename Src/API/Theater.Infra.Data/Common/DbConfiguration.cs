using Newtonsoft.Json;
using System.IO;

namespace Theater.Infra.Data.Common
{
    public class DatabaseConfiguration
    {
        private const string CONFIGURATION_FILE_NAME = "TheaterConfigurations.json";
        private static string _strWorkPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static string GetConnectionString()
        {
            DbConfiguration dbConfiguration = new DbConfiguration();
            string content = File.ReadAllText(Path.Combine(_strWorkPath, CONFIGURATION_FILE_NAME));
            if (!string.IsNullOrWhiteSpace(content))
                dbConfiguration = JsonConvert.DeserializeObject<DbConfiguration>(content);
            
            return dbConfiguration.ConnectionString;
        }
    }

    public class DbConfiguration
    {
        public string ConnectionString { get; set; }
    }
}
