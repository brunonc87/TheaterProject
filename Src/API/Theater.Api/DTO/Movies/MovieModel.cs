using Theater.Domain.Movies;

namespace Theater.Api.DTO.Movies
{
    public class MovieModel
    {
        public int Id { get; set; }
        public string Tittle { get; private set; }
        public string Description { get; private set; }
        public int Duration { get; private set; }

        public void ConvertMovieToModel(Movie movie)
        {
            Id = movie.MovieID;
            Tittle = movie.Tittle;
            Description = movie.Description;
            Duration = movie.Duration;
        }
    }
}
