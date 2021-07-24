using System.Collections.Generic;

namespace Theater.Domain.Movies
{
    public interface IMoviesService
    {
        IEnumerable<Movie> GetMovies();
        Movie GetById(int movieId);
        void AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(string tittle);
    }
}
