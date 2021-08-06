using MediatR;
using System.Collections.Generic;
using Theater.Application.Rooms.Models;

namespace Theater.Application.Rooms.Querys
{
    public class AllRoomsQuery : IRequest<IEnumerable<RoomModel>>
    {
    }
}
