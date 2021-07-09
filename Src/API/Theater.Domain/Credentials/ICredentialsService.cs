using System.Threading.Tasks;

namespace Theater.Domain.Credentials
{
    public interface ICredentialsService
    {
        bool Authenticate(string login, string password);
    }
}
