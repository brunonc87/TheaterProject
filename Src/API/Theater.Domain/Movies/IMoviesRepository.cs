using System.Collections.Generic;

namespace Theater.Domain.Movies
{
    public interface IMoviesRepository
    {
        bool Insert(Movie movie);
        bool Update(Movie movie);
        bool Delete(Movie movie);
        IEnumerable<Movie> GetAll();
        Movie GetByID(int ID);
        Movie GetByTittle(string tittle);
    }
}
