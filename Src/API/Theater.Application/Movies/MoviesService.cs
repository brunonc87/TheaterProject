using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Application.Movies.Commands;
using Theater.Domain.Movies;
using Theater.Domain.Sections;

namespace Theater.Application.Movies
{
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesRepository _movieRepository;
        private readonly ISectionsRepository _sectionRepository;

        public MoviesService(IMoviesRepository movieRepository, ISectionsRepository sectionRepository)
        {
            _movieRepository = movieRepository;
            _sectionRepository = sectionRepository;
        }

        public IEnumerable<Movie> GetMovies()
        {
            return _movieRepository.GetAll();
        }

        public Movie GetById(int movieId)
        {
            return _movieRepository.GetByID(movieId);
        }

        public void AddMovie(Movie movie)
        {
            movie.Validate();
            Movie movieInDB = _movieRepository.GetByTittle(movie.Tittle);

            if (movieInDB == null)
                _movieRepository.Insert(movie);
            else
                throw new Exception("Já existe um filme com este título cadastrado.");
        }

        public void UpdateMovie(Movie movie)
        {
            movie.Validate();
            Movie movieInDB = _movieRepository.GetByID(movie.MovieID);

            if (movieInDB == null)
                throw new Exception("Filme não encontrado.");

            Movie existingMovie = _movieRepository.GetByTittle(movie.Tittle);

            if (existingMovie != null && existingMovie.MovieID != movie.MovieID)
                throw new Exception("Já existe um filme com este título cadastrado.");

            movieInDB.CopyFrom(movie);
            _movieRepository.Update(movieInDB);
        }

        public void DeleteMovie(string tittle)
        {
            Movie movieInDB = _movieRepository.GetByTittle(tittle);

            if (movieInDB == null)
                throw new Exception("Filme não encontrado.");

            IEnumerable<Section> sections = _sectionRepository.GetByMovieID(movieInDB.MovieID);

            if (sections.Any())
                throw new Exception("Não é possível excluir um filme vinculado a alguma seção.");

            _movieRepository.Delete(movieInDB);
        }
    }
}
