using System;
using Theater.Domain.Sections;

namespace Theater.Api.DTO.Sections
{
    public class SectionAddCommand
    {
        public string MovieTittle { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Value { get; set; }
        public string AnimationType { get; set; }
        public int AudioType { get; set; }

        public Section ConvertToSection()
        {
            return new Section
            {
                StartDate = StartDate,
                Value = Value,
                AnimationType = ParseAnimationType(AnimationType),
                AudioType = (AudioType)AudioType,
                Movie = new Domain.Movies.Movie
                {
                    Tittle = MovieTittle
                },
                Room = new Domain.Rooms.Room
                {
                    Name = RoomName
                }
            };
        }

        private AnimationType ParseAnimationType(string animationType)
        {
            switch (animationType.ToUpper())
            {
                case "3D":
                    return Domain.Sections.AnimationType.D3;
                case "2D":
                default:
                    return Domain.Sections.AnimationType.D2;
            }
        }
    }
}
