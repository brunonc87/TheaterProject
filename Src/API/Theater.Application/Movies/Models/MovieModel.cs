namespace Theater.Application.Movies.Models
{
    public class MovieModel
    {
        public int Id { get; set; }
        public string Tittle { get; private set; }
        public string Description { get; private set; }
        public int Duration { get; private set; }
    }
}
