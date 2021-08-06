using AutoMapper;
using Theater.Application.Rooms.Models;
using Theater.Domain.Rooms;

namespace Theater.Application.Rooms
{
    public class RoomsMappingProfile : Profile
    {
        public RoomsMappingProfile()
        {
            CreateMap<Room, RoomModel>();
        }
    }
}
