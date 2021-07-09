using System.Collections.Generic;

namespace Theater.Domain.Sections
{
    public interface ISectionsRepository
    {
        void Insert(Section section);
        void Delete(int id);
        Section GetByID(int id);
        IEnumerable<Section> GetAll();
        IEnumerable<Section> GetByMovieID(int movieID);
    }
}
