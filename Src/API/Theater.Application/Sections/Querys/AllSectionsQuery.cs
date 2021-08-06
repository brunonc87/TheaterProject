using MediatR;
using System.Collections.Generic;
using Theater.Application.Sections.Models;

namespace Theater.Application.Sections.Querys
{
    public class AllSectionsQuery : IRequest<IEnumerable<SectionModel>>
    {
    }
}
