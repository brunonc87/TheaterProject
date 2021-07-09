using System.Collections.Generic;
using System.Linq;
using Theater.Domain.Rooms;
using Theater.Infra.Data.Common;

namespace Theater.Infra.Data.Repositories
{
    public class RoomsRepository : IRoomsRepository
    {
        private readonly TheaterContext _theaterContext;

        public RoomsRepository(TheaterContext theaterContext)
        {
            _theaterContext = theaterContext;
        }

        public IEnumerable<Room> GetAll()
        {
            return _theaterContext.Rooms;
        }

        public Room GetByName(string name)
        {
            IEnumerable<Room> rooms = _theaterContext.Rooms;
            return rooms.FirstOrDefault(r => r.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public void Insert(Room room)
        {
            _theaterContext.Rooms.Add(room);
            _theaterContext.SaveChanges();
        }
    }
}
