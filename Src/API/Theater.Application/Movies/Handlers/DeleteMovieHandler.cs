using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Movies.Commands;
using Theater.Domain.Movies;
using Theater.Domain.Sections;

namespace Theater.Application.Movies.Handlers
{
    public class DeleteMovieHandler : IRequestHandler<MovieDeleteCommand, bool>
    {
        private readonly IMoviesRepository _movieRepository;
        private readonly ISectionsRepository _sectionRepository;


        public DeleteMovieHandler(IMoviesRepository moviesRepository, ISectionsRepository sectionsRepository)
        {
            _movieRepository = moviesRepository;
            _sectionRepository = sectionsRepository;
        }

        public Task<bool> Handle(MovieDeleteCommand request, CancellationToken cancellationToken)
        {
            Movie movieInDB = _movieRepository.GetByTittle(request.MovieTittle);

            if (movieInDB == null)
                throw new Exception("Filme não encontrado.");

            IEnumerable<Section> sections = _sectionRepository.GetByMovieID(movieInDB.MovieID);

            if (sections.Any())
                throw new Exception("Não é possível excluir um filme vinculado a alguma seção.");

            return Task.FromResult(_movieRepository.Delete(movieInDB));
        }
    }
}
