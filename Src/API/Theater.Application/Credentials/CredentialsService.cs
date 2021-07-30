using System;
using System.Threading.Tasks;
using Theater.Domain.Credentials;

namespace Theater.Application.Credentials
{
    public class CredentialsService : ICredentialsService
    {
        private readonly ICredentialsRepository _credentialsRepository;
        public CredentialsService(ICredentialsRepository credentialsRepository)
        {
            _credentialsRepository = credentialsRepository;
        }

        public bool Authenticate(Credential credential)
        {
            Credential authenticatedUser = _credentialsRepository.RetrieveByLogonName(credential.Login);
            if (authenticatedUser == null)
                throw new Exception("Usuário não localizado");

            return authenticatedUser.ValidatePassword(credential.Password);
        }
    }
}
