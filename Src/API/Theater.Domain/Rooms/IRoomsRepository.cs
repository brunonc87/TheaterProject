using System.Collections.Generic;

namespace Theater.Domain.Rooms
{
    public interface IRoomsRepository
    {
        IEnumerable<Room> GetAll();
        Room GetByName(string name);
    }
}
