using System;
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

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Tittle))
                throw new Exception("Título inválido");
            if (string.IsNullOrWhiteSpace(Description))
                throw new Exception("Descrição inválida");
            if (Duration <= 0)
                throw new Exception("Tempo do filme inválido");
        }

        public void CopyFrom(Movie movie)
        {
            if (movie != null)
            {
                Tittle = movie.Tittle;
                Description = movie.Description;
                Duration = movie.Duration;
            }
        }
    }
}
