using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Movies.Commands;
using Theater.Domain.Movies;

namespace Theater.Application.Movies.Handlers
{
    public class AddMovieHandler : IRequestHandler<MovieAddCommand, bool>
    {
        private readonly IMoviesRepository _movieRepository;
        private readonly IMapper _mapper;

        public AddMovieHandler(IMoviesRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public Task<bool> Handle(MovieAddCommand request, CancellationToken cancellationToken)
        {
            Movie movieInDB = _movieRepository.GetByTittle(request.Tittle);

            if (movieInDB != null)
                throw new Exception("Já existe um filme com este título cadastrado.");

            return Task.FromResult(_movieRepository.Insert(_mapper.Map<Movie>(request)));

        }
    }
}
