using System.Collections.Generic;

namespace Theater.Domain.Movies
{
    public interface IMoviesRepository
    {
        void Insert(Movie movie);
        void Update(Movie movie);
        void Delete(Movie movie);
        IEnumerable<Movie> GetAll();
        Movie GetByID(int ID);
        Movie GetByTittle(string tittle);
    }
}
