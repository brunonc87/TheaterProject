using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Theater.Application.Rooms.Handlers;
using Theater.Application.Rooms.Models;
using Theater.Domain.Rooms;
using Theater.Infra.Data.Repositories;
using Theater.Integration.Tests.Common;

namespace Theater.Integration.Tests.Application
{
    [TestClass]
    public class RoomsHandlersTests : TheaterIntegrationBase
    {
        private IRoomsRepository _roomsRepository;
        private GetAllRoomsHandler _getAllRoomsHandler;

        [TestInitialize]
        public void Initialize()
        {
            base.Reset();
            base.ConfigureAutomapper();
            _roomsRepository = new RoomsRepository(_theaterContext);
            _getAllRoomsHandler = new GetAllRoomsHandler(_roomsRepository, _mapper);
        }

        [TestMethod]
        public void GetAllRoomsHandler_Should_Return_Rooms_On_Database()
        {
            Room room1 = new Room { Name = "Sala 1", SeatsNumber = 14 };
            Room room2 = new Room { Name = "Sala 7", SeatsNumber = 22 };
            new RoomsRepository(_theaterContext).Insert(room1);
            new RoomsRepository(_theaterContext).Insert(room2);


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
