using System.Collections.Generic;

namespace Theater.Domain.Rooms
{
    public interface IRoomsService
    {
        IEnumerable<Room> GetRooms();
    }
}
