using Theater.Domain.Movies;

namespace Theater.Api.DTO.Movies
{
    public class MovieAddCommand
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public Movie ConvertToMovie()
        {
            return new Movie()
            {
                Tittle = Tittle,
                Description = Description,
                Duration = Duration
            };
        }
    }
}
