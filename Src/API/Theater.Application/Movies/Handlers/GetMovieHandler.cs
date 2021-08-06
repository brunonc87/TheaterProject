using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Movies.Models;
using Theater.Application.Movies.Querys;
using Theater.Domain.Movies;

namespace Theater.Application.Movies.Handlers
{
    public class GetMovieHandler : IRequestHandler<MovieQuery, MovieModel>
    {
        private readonly IMoviesRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieHandler(IMoviesRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public Task<MovieModel> Handle(MovieQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mapper.Map<MovieModel>(_movieRepository.GetByID(request.MovieId)));
        }
    }
}
