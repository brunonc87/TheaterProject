using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Application.Sections;
using Theater.Application.Sections.Commands;
using Theater.Application.Sections.Handlers;
using Theater.Application.Sections.Models;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;

namespace Theater.Unit.Tests.Application
{
    [TestClass]
    public class SectionsHandlersTests
    {
        private Mock<ISectionsRepository> _sectionsRepository;
        private Mock<IMoviesRepository> _moviesRepository;
        private Mock<IRoomsRepository> _roomsRepository;
        private IMapper _mapper;

        [TestInitialize]
        public void Initiralize()
        {
            _sectionsRepository = new Mock<ISectionsRepository>();
            _moviesRepository = new Mock<IMoviesRepository>();
            _roomsRepository = new Mock<IRoomsRepository>();
            _mapper = new MapperConfiguration(mc => mc.AddProfile(new SectionsMappingProfile())).CreateMapper();
        }

        [TestMethod]
        public void GetAllSectionsHandler_Should_Return_Sections_On_Database()
        {
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room1OnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section section1OnDb = new Section { SectionID = 300, StartDate = DateTime.Now, Value = 12, AnimationType = AnimationType.D3,
                                                 AudioType = AudioType.Original, Movie = movie1OnDb, Room = room1OnDb };
            
            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Room room2OnDb = new Room { RoomID = 3000, Name = "Sala 1", SeatsNumber = 23 };
            Section section2OnDb = new Section { RoomID = 10000, StartDate = DateTime.Now, Value = 15, AnimationType = AnimationType.D2,
                                                 AudioType = AudioType.Dubbed, Movie = movie2OnDb, Room = room2OnDb };

            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { section1OnDb, section2OnDb }).Verifiable();

            GetAllSectionsHandler getAllSectionsHandler = new GetAllSectionsHandler(_sectionsRepository.Object, _mapper);
            IEnumerable<SectionModel> sectionsOnDB = getAllSectionsHandler.Handle(new Theater.Application.Sections.Querys.AllSectionsQuery(), new System.Threading.CancellationToken()).Result;

            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(2);
            sectionsOnDB.First().MovieTittle.Should().Be(movie1OnDb.Tittle);
            sectionsOnDB.First().RoomName.Should().Be(room1OnDb.Name);
            sectionsOnDB.First().Value.Should().Be(section1OnDb.Value);
            sectionsOnDB.Last().MovieTittle.Should().Be(movie2OnDb.Tittle);
            sectionsOnDB.Last().RoomName.Should().Be(room2OnDb.Name);
            sectionsOnDB.Last().Value.Should().Be(section2OnDb.Value);
            _sectionsRepository.Verify();
        }

        [TestMethod]
        public void DeleteSectionHandler_Should_Remove_Section_From_Database_When_Have_More_Than_10_Days_To_Section_Date()
        {
            int sectionId = 300;
            Movie movieOnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room roomOnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = sectionId, StartDate = DateTime.Now.AddDays(11), Value = 12, AnimationType = AnimationType.D3,
                                                AudioType = AudioType.Original, Movie = movieOnDb, Room = roomOnDb };
                        
            _sectionsRepository.Setup(x => x.GetByID(It.Is<int>(s => s == sectionId))).Returns(sectionOnDb).Verifiable();
            _sectionsRepository.Setup(x => x.Delete(It.Is<int>(s => s == sectionId))).Returns(true).Verifiable();

            DeleteSectionHandler deleteSectionHandler = new DeleteSectionHandler(_sectionsRepository.Object);
            bool result = deleteSectionHandler.Handle(new SectionDeleteCommand(sectionId), new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _sectionsRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteSectionHandler_Should_Throw_Exception_When_Have_Less_Than_10_Days_To_Section_Date()
        {
            int sectionId = 300;
            Movie movieOnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room roomOnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddDays(9), Value = 12, AnimationType = AnimationType.D3, 
                                                AudioType = AudioType.Original, Movie = movieOnDb, Room = roomOnDb };
            
            _sectionsRepository.Setup(x => x.GetByID(It.Is<int>(s => s == sectionId))).Returns(sectionOnDb).Verifiable();

            DeleteSectionHandler deleteSectionHandler = new DeleteSectionHandler(_sectionsRepository.Object);
            Action act = () => deleteSectionHandler.Handle(new SectionDeleteCommand(sectionId), new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Não é possível remover uma seção se estiver tão proxima de ocorrer.");
            _sectionsRepository.Verify();
            _sectionsRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void AddSectionHandler_Should_Add_Section_On_Database()
        {
            Movie movie = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            SectionAddCommand section = new SectionAddCommand { StartDate = DateTime.Now.AddDays(2), Value = 12, AnimationType = "3D", AudioType = 1, MovieTittle = movie.Tittle, RoomName = room.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == section.MovieTittle))).Returns(movie).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == section.RoomName))).Returns(room).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Verifiable();
            _sectionsRepository.Setup(x => x.Insert(It.Is<Section>(s => s.Movie.Tittle == section.MovieTittle && s.Room.Name == section.RoomName &&
                                                                        s.StartDate == section.StartDate && s.Value == section.Value &&
                                                                        s.AnimationType == (section.AnimationType == "2D" ? AnimationType.D2 : AnimationType.D3) &&
                                                                        s.AudioType == (AudioType)section.AudioType))).Returns(true).Verifiable();

            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            bool result = addSectionHandler.Handle(section, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.VerifyAll();
        }

        [TestMethod]
        public void AddSectionHandler_Should_Throw_Exception_When_Another_Section_is_Created_In_Same_Room_With_StartDate_After_Section_Start_And_Before_Section_End()
        {
            Room roomOnDB = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };            
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddMinutes(10), Value = 12, AnimationType = AnimationType.D3,
                                                AudioType = AudioType.Original, Movie = movie1OnDb, Room = roomOnDB };

            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            SectionAddCommand sectionAddCommand = new SectionAddCommand { StartDate = DateTime.Now.AddMinutes(100), Value = 15, AnimationType = "2D",
                                                                          AudioType = 2, MovieTittle = movie2OnDb.Tittle, RoomName = roomOnDB.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == sectionAddCommand.MovieTittle))).Returns(movie2OnDb).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == sectionAddCommand.RoomName))).Returns(roomOnDB).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { sectionOnDb }).Verifiable();

            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            Action act = () => addSectionHandler.Handle(sectionAddCommand, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Não é possivel criar uma seção no horario que já existe outra seção.");
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.Verify();
            _sectionsRepository.VerifyNoOtherCalls();            
        }

        [TestMethod]
        public void AddSectionHandler_Should_Throw_Exception_When_Another_Section_is_Created_In_Same_Room_With_EndDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room roomOnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddMinutes(100), Value = 12, AnimationType = AnimationType.D3, 
                                                AudioType = AudioType.Original, Movie = movie1OnDb, Room = roomOnDb };

            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            SectionAddCommand sectionAddCommand = new SectionAddCommand { StartDate = DateTime.Now.AddMinutes(10), Value = 15, AnimationType = "2D",
                                                                          AudioType = 2, MovieTittle = movie2OnDb.Tittle, RoomName = roomOnDb.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == sectionAddCommand.MovieTittle))).Returns(movie2OnDb).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == sectionAddCommand.RoomName))).Returns(roomOnDb).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { sectionOnDb }).Verifiable();

            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            Action act = () => addSectionHandler.Handle(sectionAddCommand, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Não é possivel criar uma seção no horario que já existe outra seção.");
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.Verify();
            _sectionsRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void AddSectionHandler_Should_Add_Section_When_Another_Section_is_Created_In_Other_Room_With_StartDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room1OnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddMinutes(10), Value = 12, AnimationType = AnimationType.D3,
                                                AudioType = AudioType.Original, Movie = movie1OnDb, Room = room1OnDb };

            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Room room2OnDb = new Room { RoomID = 750, Name = "Sala 9", SeatsNumber = 40 };
            SectionAddCommand sectionAddCommand = new SectionAddCommand { StartDate = DateTime.Now.AddMinutes(100), Value = 15, AnimationType = "2D",
                                                                          AudioType = 2, MovieTittle = movie2OnDb.Tittle, RoomName = room2OnDb.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == sectionAddCommand.MovieTittle))).Returns(movie2OnDb).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == sectionAddCommand.RoomName))).Returns(room2OnDb).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { sectionOnDb }).Verifiable();
            _sectionsRepository.Setup(x => x.Insert(It.Is<Section>(s => s.Movie.Tittle == sectionAddCommand.MovieTittle && s.Room.Name == sectionAddCommand.RoomName &&
                                                                        s.StartDate == sectionAddCommand.StartDate && s.Value == sectionAddCommand.Value &&
                                                                        s.AnimationType == (sectionAddCommand.AnimationType == "2D" ? AnimationType.D2 : AnimationType.D3) &&
                                                                        s.AudioType == (AudioType)sectionAddCommand.AudioType))).Returns(true).Verifiable();


            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            bool result = addSectionHandler.Handle(sectionAddCommand, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.VerifyAll();
        }

        [TestMethod]
        public void AddSectionHandler_Should_Add_Section_When_Another_Section_is_Created_In_Other_Room_With_EndDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room1OnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddMinutes(100), Value = 12, AnimationType = AnimationType.D3,
                                                AudioType = AudioType.Original, Movie = movie1OnDb, Room = room1OnDb };

            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Room room2OnDb = new Room { RoomID = 750, Name = "Sala 9", SeatsNumber = 40 };
            SectionAddCommand sectionAddCommand = new SectionAddCommand { StartDate = DateTime.Now.AddMinutes(10), Value = 15, AnimationType = "2D",
                                                                          AudioType = 2, MovieTittle = movie2OnDb.Tittle, RoomName = room2OnDb.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == sectionAddCommand.MovieTittle))).Returns(movie2OnDb).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == sectionAddCommand.RoomName))).Returns(room2OnDb).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { sectionOnDb }).Verifiable();
            _sectionsRepository.Setup(x => x.Insert(It.Is<Section>(s => s.Movie.Tittle == sectionAddCommand.MovieTittle && s.Room.Name == sectionAddCommand.RoomName &&
                                                                        s.StartDate == sectionAddCommand.StartDate && s.Value == sectionAddCommand.Value &&
                                                                        s.AnimationType == (sectionAddCommand.AnimationType == "2D" ? AnimationType.D2 : AnimationType.D3) &&
                                                                        s.AudioType == (AudioType)sectionAddCommand.AudioType))).Returns(true).Verifiable();

            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            bool result = addSectionHandler.Handle(sectionAddCommand, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.VerifyAll();
        }

        [TestMethod]
        public void AddSectionHandler_Should_Add_Section_When_Another_Section_is_Created_In_Same_Room_With_StartDate_After_Section_End()
        {
            Room roomOnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };            
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddMinutes(140), Value = 12, AnimationType = AnimationType.D3, 
                                                AudioType = AudioType.Original, Movie = movie1OnDb, Room = roomOnDb };

            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            SectionAddCommand sectionAddCommand = new SectionAddCommand { StartDate = DateTime.Now.AddMinutes(10), Value = 15, AnimationType = "2D", 
                                                                          AudioType = 2, MovieTittle = movie2OnDb.Tittle, RoomName = roomOnDb.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == sectionAddCommand.MovieTittle))).Returns(movie2OnDb).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == sectionAddCommand.RoomName))).Returns(roomOnDb).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { sectionOnDb }).Verifiable();
            _sectionsRepository.Setup(x => x.Insert(It.Is<Section>(s => s.Movie.Tittle == sectionAddCommand.MovieTittle && s.Room.Name == sectionAddCommand.RoomName &&
                                                                        s.StartDate == sectionAddCommand.StartDate && s.Value == sectionAddCommand.Value &&
                                                                        s.AnimationType == (sectionAddCommand.AnimationType == "2D" ? AnimationType.D2 : AnimationType.D3) &&
                                                                        s.AudioType == (AudioType)sectionAddCommand.AudioType))).Returns(true).Verifiable();

            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            bool result = addSectionHandler.Handle(sectionAddCommand, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.VerifyAll();
        }

        [TestMethod]
        public void AddSectionHandler_Should_Add_Section_When_Another_Section_is_Created_In_Same_Room_With_EndDate_Before_Section_Start()
        {
            Movie movie1OnDb = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room roomOnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now.AddMinutes(10), Value = 12, AnimationType = AnimationType.D3,
                                                AudioType = AudioType.Original, Movie = movie1OnDb, Room = roomOnDb };

            Movie movie2OnDb = new Movie { MovieID = 600, Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            SectionAddCommand sectionAddCommand = new SectionAddCommand { StartDate = DateTime.Now.AddMinutes(165), Value = 15, AnimationType = "2D", 
                                                                            AudioType = 2, MovieTittle = movie2OnDb.Tittle, RoomName = roomOnDb.Name };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == sectionAddCommand.MovieTittle))).Returns(movie2OnDb).Verifiable();
            _roomsRepository.Setup(x => x.GetByName(It.Is<string>(r => r == sectionAddCommand.RoomName))).Returns(roomOnDb).Verifiable();
            _sectionsRepository.Setup(x => x.GetAll()).Returns(new List<Section> { sectionOnDb }).Verifiable();
            _sectionsRepository.Setup(x => x.Insert(It.Is<Section>(s => s.Movie.Tittle == sectionAddCommand.MovieTittle && s.Room.Name == sectionAddCommand.RoomName &&
                                                                        s.StartDate == sectionAddCommand.StartDate && s.Value == sectionAddCommand.Value &&
                                                                        s.AnimationType == (sectionAddCommand.AnimationType == "2D" ? AnimationType.D2 : AnimationType.D3) &&
                                                                        s.AudioType == (AudioType)sectionAddCommand.AudioType))).Returns(true).Verifiable();

            AddSectionHandler addSectionHandler = new AddSectionHandler(_sectionsRepository.Object, _moviesRepository.Object, _roomsRepository.Object, _mapper);
            bool result = addSectionHandler.Handle(sectionAddCommand, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.Verify();
            _roomsRepository.Verify();
            _sectionsRepository.VerifyAll();
        } 
    }
}
