using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Credentials.Commands;
using Theater.Application.Credentials.Models;
using Theater.Domain.Credentials;
using Theater.Infra.Settings;

namespace Theater.Application.Credentials.Handlers
{
    public class CredentialHandler : IRequestHandler<CredentialCommand, LoginInfoModel>
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IMapper _mapper;

        public CredentialHandler(ICredentialsRepository credentialsRepository, IMapper mapper)
        {
            _credentialsRepository = credentialsRepository;
            _mapper = mapper;
        }
        public Task<LoginInfoModel> Handle(CredentialCommand request, CancellationToken cancellationToken)
        {
            Credential userFromAuthentication = _mapper.Map<Credential>(request);
            Credential authenticatedUser = _credentialsRepository.RetrieveByLogonName(userFromAuthentication.Login);
            if (authenticatedUser == null)
                throw new Exception("Usuário não localizado");

            if (authenticatedUser.ValidatePassword(userFromAuthentication.Password))
            {
                return Task.FromResult(new LoginInfoModel
                {
                    Login = userFromAuthentication.Login,
                    Token = GenerateToken(userFromAuthentication.Login)
                });
            }
            else
                throw new Exception("Senha inválida");
        }

        private string GenerateToken(string userLogin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userLogin),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Settings.Secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
