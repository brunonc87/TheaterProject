using AutoMapper;
using Theater.Application.Rooms.Models;
using Theater.Domain.Rooms;

namespace Theater.Application.Rooms
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Room, RoomModel>();
        }
    }
}
