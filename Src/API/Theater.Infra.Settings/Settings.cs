using System.Text;

namespace Theater.Infra.Settings
{
    public static class Settings
    {
        private const string SECRET = "miras7d8863b48e197b9287d492b708e";
        public static byte[] Secret { get { return Encoding.ASCII.GetBytes(SECRET); } }
    }    
}
