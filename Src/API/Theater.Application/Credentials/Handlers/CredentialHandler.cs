using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Credentials.Commands;
using Theater.Domain.Credentials;

namespace Theater.Application.Credentials.Handlers
{
    public class CredentialHandler : IRequestHandler<CredentialCommand, bool>
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IMapper _mapper;

        public CredentialHandler(ICredentialsRepository credentialsRepository, IMapper mapper)
        {
            _credentialsRepository = credentialsRepository;
            _mapper = mapper;
        }
        public Task<bool> Handle(CredentialCommand request, CancellationToken cancellationToken)
        {
            Credential userFromAuthentication = _mapper.Map<Credential>(request);
            Credential authenticatedUser = _credentialsRepository.RetrieveByLogonName(userFromAuthentication.Login);
            if (authenticatedUser == null)
                throw new Exception("Usuário não localizado");

            return Task.FromResult(authenticatedUser.ValidatePassword(userFromAuthentication.Password));
        }
    }
}
