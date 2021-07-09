using Theater.Domain.Movies;

namespace Theater.Api.DTO.Movies
{
    public class MovieUpdateCommand
    {
        public int MovieID { get; set; }
        public string Tittle { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public Movie ConvertToMovie()
        {
            return new Movie()
            {
                MovieID = MovieID,
                Tittle = Tittle,
                Description = Description,
                Duration = Duration
            };
        }
    }
}
