using System;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;

namespace Theater.Domain.Sections
{
    public class Section
    {
        private Movie _movie;
        private Room _room;

        public int SectionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get { return this.StartDate.AddMinutes(_movie.Duration); } }
        public decimal Value { get; set; }
        public AnimationType AnimationType { get; set; }
        public AudioType AudioType { get; set; }
        public int? MovieID { get; set; }
        public Movie Movie
        {
            get { return _movie; }
            set
            {
                _movie = value;
                if (_movie == null)
                    MovieID = null;
                else
                    MovieID = _movie.MovieID;
            }
        }
        public int? RoomID { get; set; }
        public Room Room
        {
            get { return _room; }
            set
            {
                _room = value;
                if (_room == null)
                    RoomID = null;
                else
                    RoomID = _room.RoomID;
            }
        }

        public bool CanBeDeleted()
        {
            return (StartDate - DateTime.Now).TotalDays >= 10;
        }             
    }
}
