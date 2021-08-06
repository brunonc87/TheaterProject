using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Movies.Models;
using Theater.Application.Movies.Querys;
using Theater.Domain.Movies;

namespace Theater.Application.Movies.Handlers
{
    public class GetAllMoviesHandler : IRequestHandler<AllMoviesQuery, IEnumerable<MovieModel>>
    {
        private readonly IMoviesRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetAllMoviesHandler(IMoviesRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<MovieModel>> Handle(AllMoviesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mapper.Map<IEnumerable<MovieModel>>(_movieRepository.GetAll()));
        }
    }
}
