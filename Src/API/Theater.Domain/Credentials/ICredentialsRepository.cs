using System.Threading.Tasks;

namespace Theater.Domain.Credentials
{
    public interface ICredentialsRepository
    {
        Credential RetrieveByLogonName(string logonName);
        void AddCredential(Credential credential);
    }
}
