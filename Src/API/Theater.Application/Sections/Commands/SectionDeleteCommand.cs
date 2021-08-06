using MediatR;

namespace Theater.Application.Sections.Commands
{
    public class SectionDeleteCommand : IRequest<bool>
    {
        public SectionDeleteCommand(int id)
        {
            SectionId = id;
        }

        public int SectionId { get; set; }
    }
}
