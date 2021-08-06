using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Movies.Commands;
using Theater.Domain.Movies;

namespace Theater.Application.Movies.Handlers
{
    public class UpdateMovieHandler : IRequestHandler<MovieUpdateCommand, bool>
    {
        private readonly IMoviesRepository _movieRepository;
        private readonly IMapper _mapper;

        public UpdateMovieHandler(IMoviesRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }


        public Task<bool> Handle(MovieUpdateCommand request, CancellationToken cancellationToken)
        {
            Movie movieInDB = _movieRepository.GetByID(request.ID);

            if (movieInDB == null)
                throw new Exception("Filme não encontrado.");

            Movie existingMovie = _movieRepository.GetByTittle(request.Tittle);

            if (existingMovie != null && existingMovie.MovieID != request.ID)
                throw new Exception("Já existe um filme com este título cadastrado.");

            return Task.FromResult(_movieRepository.Update(_mapper.Map(request, movieInDB)));
        }
    }
}
