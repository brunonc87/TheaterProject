using System.Collections.Generic;

namespace Theater.Domain.Movies
{
    public interface IMoviesService
    {
        IEnumerable<Movie> GetMovies();
        Movie GetByTittle(string tittle);
        void AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(string tittle);
    }
}
