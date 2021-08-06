using System.Collections.Generic;
using Theater.Domain.Sections;

namespace Theater.Domain.Movies
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Tittle { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public List<Section> Sections { get; set; }
    }
}
