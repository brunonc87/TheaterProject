using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Api.DTO.Rooms;
using Theater.Domain.Rooms;

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomsService _roomService;

        public RoomController(IRoomsService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<RoomModel> roomModels = new List<RoomModel>();
                IEnumerable<Room> rooms = _roomService.GetRooms();

                foreach (Room room in rooms)
                {
                    RoomModel roomModel = new RoomModel();
                    roomModel.ConvertRoomToModel(room);
                    roomModels.Add(roomModel);
                }

                return Ok(roomModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
