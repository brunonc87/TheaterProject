using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Application.Rooms;
using Theater.Application.Rooms.Handlers;
using Theater.Application.Rooms.Models;
using Theater.Domain.Rooms;

namespace Theater.Unit.Tests.Application
{
    [TestClass]
    public class RoomsHandlersTests
    {
        private Mock<IRoomsRepository> _roomsRepository;
        private GetAllRoomsHandler _getAllRoomsHandler;
        private IMapper _mapper;

        [TestInitialize]
        public void Initialize()
        {
            _roomsRepository = new Mock<IRoomsRepository>();
            _mapper = new MapperConfiguration(mc => mc.AddProfile(new RoomsMappingProfile())).CreateMapper();
            _getAllRoomsHandler = new GetAllRoomsHandler(_roomsRepository.Object, _mapper);
        }

        [TestMethod]
        public void GetAllRoomsHandler_Should_Return_Rooms_On_Database()
        {
            Room room1 = new Room { RoomID = 1, Name = "Sala 1", SeatsNumber = 14 };
            Room room2 = new Room { RoomID = 2, Name = "Sala 7", SeatsNumber = 22 };

            _roomsRepository.Setup(x => x.GetAll()).Returns(new List<Room> { room1, room2 }).Verifiable();

            IEnumerable<RoomModel> roomsOnDB = _getAllRoomsHandler.Handle(new Theater.Application.Rooms.Querys.AllRoomsQuery(), new System.Threading.CancellationToken()).Result;

            roomsOnDB.Should().NotBeEmpty();
            roomsOnDB.Should().HaveCount(2);
            roomsOnDB.First().Name.Should().Be(room1.Name);
            roomsOnDB.First().SeatsNumber.Should().Be(room1.SeatsNumber);
            roomsOnDB.Last().Name.Should().Be(room2.Name);
            roomsOnDB.Last().SeatsNumber.Should().Be(room2.SeatsNumber);
        }


    }
}
