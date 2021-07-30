using System;

namespace Theater.Application.Sections.Models
{
    public class SectionModel
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Value { get; set; }
        public string AnimationType { get; set; }
        public int AudioType { get; set; }
        public string MovieTittle { get; set; }
        public int Duration { get; set; }
        public string RoomName { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
