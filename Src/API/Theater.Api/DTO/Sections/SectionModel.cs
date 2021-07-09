using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Domain.Sections;

namespace Theater.Api.DTO.Sections
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

        public void ConsertSectionToModel(Section section)
        {
            ID = section.SectionID;
            StartDate = section.StartDate;
            Value = section.Value;
            AnimationType = ParseAnimationType(section.AnimationType);
            AudioType = section.AudioType.GetHashCode();
            MovieTittle = section.Movie.Tittle;
            Duration = section.Movie.Duration;
            RoomName = section.Room.Name;
            NumberOfSeats = section.Room.SeatsNumber;
        }

        private string ParseAnimationType(AnimationType animationType)
        {
            switch (animationType)
            {
                case Domain.Sections.AnimationType.D3:
                    return "3D";
                case Domain.Sections.AnimationType.D2:
                default:
                    return "2D";
            }
        }
    }
}
