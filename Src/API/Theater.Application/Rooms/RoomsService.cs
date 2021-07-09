using System.Collections.Generic;
using Theater.Domain.Rooms;

namespace Theater.Application.Rooms
{
    public class RoomsService : IRoomsService
    {
        private readonly IRoomsRepository _roomRepository;

        public RoomsService(IRoomsRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<Room> GetRooms()
        {
            return _roomRepository.GetAll();
        }
    }
}
