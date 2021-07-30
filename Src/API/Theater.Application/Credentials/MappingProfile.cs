using AutoMapper;
using Theater.Application.Credentials.Commands;
using Theater.Domain.Credentials;

namespace Theater.Application.Credentials
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CredentialCommand, Credential>();
        }
    }
}
