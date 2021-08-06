using MediatR;
using Theater.Application.Movies.Models;

namespace Theater.Application.Movies.Querys
{
    public class MovieQuery : IRequest<MovieModel>
    {
        public MovieQuery(int id)
        {
            MovieId = id;
        }

        public int MovieId { get; set; }
    }
}
