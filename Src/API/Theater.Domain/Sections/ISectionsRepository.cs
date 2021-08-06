using System.Collections.Generic;

namespace Theater.Domain.Sections
{
    public interface ISectionsRepository
    {
        bool Insert(Section section);
        bool Delete(int id);
        Section GetByID(int id);
        IEnumerable<Section> GetAll();
        IEnumerable<Section> GetByMovieID(int movieID);
    }
}
