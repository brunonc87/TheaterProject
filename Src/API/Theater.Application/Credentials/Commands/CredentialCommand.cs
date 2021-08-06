using MediatR;

namespace Theater.Application.Credentials.Commands
{
    public class CredentialCommand : IRequest<bool>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
