using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Theater.Application.Rooms.Models;
using Theater.Domain.Rooms;

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomsService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomsService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        // GET: api/<RoomController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Room> rooms = _roomService.GetRooms();

                return Ok(_mapper.Map<IEnumerable<RoomModel>>(rooms));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
