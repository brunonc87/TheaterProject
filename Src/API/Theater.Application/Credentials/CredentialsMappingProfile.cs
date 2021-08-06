using AutoMapper;
using Theater.Application.Credentials.Commands;
using Theater.Domain.Credentials;

namespace Theater.Application.Credentials
{
    public class CredentialsMappingProfile : Profile
    {
        public CredentialsMappingProfile()
        {
            CreateMap<CredentialCommand, Credential>();
        }
    }
}
