using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Theater.Domain.Sections;
using Theater.Infra.Data.Common;

namespace Theater.Infra.Data.Repositories
{
    public class SectionsRepository : ISectionsRepository
    {
        private readonly TheaterContext _theaterContext;

        public SectionsRepository(TheaterContext theaterContext)
        {
            _theaterContext = theaterContext;
        }

        public bool Delete(int id)
        {
            Section section = _theaterContext.Sections.FirstOrDefault(s => s.SectionID == id);
            _theaterContext.Sections.Remove(section);
            return _theaterContext.SaveChanges() > 0;
        }

        public IEnumerable<Section> GetAll()
        {
            return _theaterContext.Sections.Include(s => s.Movie).Include(s => s.Room);
        }

        public Section GetByID(int id)
        {
            return _theaterContext.Sections.FirstOrDefault(s => s.SectionID == id);
        }

        public IEnumerable<Section> GetByMovieID(int movieID)
        {
            return _theaterContext.Sections.Include(s => s.Movie).Where(s => s.Movie.MovieID == movieID);
        }

        public bool Insert(Section section)
        {
            _theaterContext.Sections.Add(section);
            return _theaterContext.SaveChanges() > 0;
        }
    }
}
