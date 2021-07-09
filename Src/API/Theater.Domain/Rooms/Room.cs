using System.Collections.Generic;
using Theater.Domain.Sections;

namespace Theater.Domain.Rooms
{
    public class Room
    {
        public int RoomID { get; set; }
        public string Name { get; set; }
        public int SeatsNumber { get; set; }
        public List<Section> Sections { get; set; }
    }
}
