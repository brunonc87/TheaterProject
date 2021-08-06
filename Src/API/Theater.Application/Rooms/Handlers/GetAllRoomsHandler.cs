using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Rooms.Models;
using Theater.Application.Rooms.Querys;
using Theater.Domain.Rooms;

namespace Theater.Application.Rooms.Handlers
{
    public class GetAllRoomsHandler : IRequestHandler<AllRoomsQuery, IEnumerable<RoomModel>>
    {
        private readonly IRoomsRepository _roomRepository;
        private readonly IMapper _mapper;

        public GetAllRoomsHandler(IRoomsRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<RoomModel>> Handle(AllRoomsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mapper.Map<IEnumerable<RoomModel>>(_roomRepository.GetAll()));
        }
    }
}
