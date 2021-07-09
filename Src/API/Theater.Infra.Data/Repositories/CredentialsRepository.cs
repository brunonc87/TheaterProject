using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Domain.Credentials;
using Theater.Infra.Data.Common;

namespace Theater.Infra.Data.Repositories
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private readonly TheaterContext _theaterContext;

        public CredentialsRepository(TheaterContext theaterContext)
        {
            _theaterContext = theaterContext;
        }

        public Credential RetrieveByLogonName(string logonName)
        {
            IEnumerable<Credential> credentials = _theaterContext.Credentials;
            
            return credentials.FirstOrDefault(c => c.Login.Equals(logonName, StringComparison.InvariantCultureIgnoreCase));
        }

        public void AddCredential(Credential credential)
        {
            _theaterContext.Credentials.Add(credential);
            _theaterContext.SaveChanges();
        }
    }
}
