using MediatR;
using System.Collections.Generic;
using Theater.Application.Movies.Models;

namespace Theater.Application.Movies.Querys
{
    public class AllMoviesQuery : IRequest<IEnumerable<MovieModel>>
    {
    }
}
