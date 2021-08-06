using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Domain.Movies;
using Theater.Infra.Data.Common;

namespace Theater.Infra.Data.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly TheaterContext _theaterContext;

        public MoviesRepository(TheaterContext theaterContext)
        {
            _theaterContext = theaterContext;
        }

        public bool Delete(Movie movie)
        {
            _theaterContext.Remove(movie);
            return _theaterContext.SaveChanges() > 0;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _theaterContext.Movies;
        }

        public Movie GetByID(int ID)
        {
            return _theaterContext.Movies.FirstOrDefault(m => m.MovieID == ID);
        }

        public Movie GetByTittle(string tittle)
        {
            IEnumerable<Movie> movies = _theaterContext.Movies;

            return movies.FirstOrDefault(m => m.Tittle.Equals(tittle, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool Insert(Movie movie)
        {
            _theaterContext.Movies.Add(movie);
            return _theaterContext.SaveChanges() > 0;
        }

        public bool Update(Movie movie)
        {
            _theaterContext.Attach(movie);
            _theaterContext.Entry(movie).State = EntityState.Modified;
            return _theaterContext.SaveChanges() > 0;
        }
    }
}
