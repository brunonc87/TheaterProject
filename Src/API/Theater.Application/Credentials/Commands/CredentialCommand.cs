using MediatR;
using Theater.Application.Credentials.Models;
using Theater.Domain.Credentials;

namespace Theater.Application.Credentials.Commands
{
    public class CredentialCommand : IRequest<LoginInfoModel>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
