using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Sections.Models;
using Theater.Application.Sections.Querys;
using Theater.Domain.Sections;

namespace Theater.Application.Sections.Handlers
{
    public class GetAllSectionsHandler : IRequestHandler<AllSectionsQuery, IEnumerable<SectionModel>>
    {
        private readonly ISectionsRepository _sectionRepository;
        private readonly IMapper _mapper;

        public GetAllSectionsHandler(ISectionsRepository sectionRepository, IMapper mapper)
        {
            _sectionRepository = sectionRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<SectionModel>> Handle(AllSectionsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mapper.Map<IEnumerable<SectionModel>>(_sectionRepository.GetAll()));
        }
    }
}
