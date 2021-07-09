using System.Collections.Generic;

namespace Theater.Domain.Sections
{
    public interface ISectionsService
    {
        void AddSection(Section section);
        void RemoveSection(int sectionID);
        IEnumerable<Section> GetSections();
    }
}
