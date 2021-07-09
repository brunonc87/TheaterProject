using Theater.Domain.Rooms;

namespace Theater.Api.DTO.Rooms
{
    public class RoomModel
    {
        public string Name { get; set; }
        public int SeatsNumber { get; set; }

        public void ConvertRoomToModel(Room room)
        {
            Name = room.Name;
            SeatsNumber = room.SeatsNumber;
        }
    }
}
