using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Sections.Commands;
using Theater.Domain.Sections;

namespace Theater.Application.Sections.Handlers
{
    public class DeleteSectionHandler : IRequestHandler<SectionDeleteCommand, bool>
    {
        private readonly ISectionsRepository _sectionRepository;

        public DeleteSectionHandler(ISectionsRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public Task<bool> Handle(SectionDeleteCommand request, CancellationToken cancellationToken)
        {
            Section sectionOnDB = _sectionRepository.GetByID(request.SectionId);

            if (!sectionOnDB.CanBeDeleted())
                throw new Exception("Não é possível remover uma seção se estiver tão proxima de ocorrer.");

            return Task.FromResult(_sectionRepository.Delete(request.SectionId));
        }
    }
}
