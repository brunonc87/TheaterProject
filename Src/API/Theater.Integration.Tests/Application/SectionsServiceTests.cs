using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Application.Sections;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;
using Theater.Infra.Data.Repositories;
using Theater.Integration.Tests.Common;

namespace Theater.Integration.Tests.Application
{
    [TestClass]
    public class SectionsServiceTests : TheaterIntegrationBase
    {
        private ISectionsRepository _sectionsRepository;
        private IMoviesRepository _moviesRepository;
        private IRoomsRepository _roomsRepository;
        private ISectionsService _sectionsService;

        [TestInitialize]
        public void Initiralize()
        {
            base.Reset();

            _sectionsRepository = new SectionsRepository(_theaterContext);
            _moviesRepository = new MoviesRepository(_theaterContext);
            _roomsRepository = new RoomsRepository(_theaterContext);
            _sectionsService = new SectionsService(_sectionsRepository, _moviesRepository, _roomsRepository);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Return_Sections_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now, Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Room room2 = new Room { Name = "Sala 1", SeatsNumber = 23 };
            Section section2 = new Section { StartDate = DateTime.Now, Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            new RoomsRepository(_theaterContext).Insert(room2);
            section2.Movie = _moviesRepository.GetAll().Last();
            section2.Room = new RoomsRepository(_theaterContext).GetAll().Last();
            _sectionsRepository.Insert(section2);

            IEnumerable<Section> sectionsOnDB = _sectionsService.GetSections();

            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(2);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
            sectionsOnDB.Last().Movie.Tittle.Should().Be(movie2.Tittle);
            sectionsOnDB.Last().Room.Name.Should().Be(room2.Name);
            sectionsOnDB.Last().Value.Should().Be(section2.Value);
        }

        [TestMethod]
        public void SectionsService_RemoveSection_Should_Remove_Section_From_Database_When_Have_More_Than_10_Days_To_Section_Date()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddDays(11), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);
            int sectionID = _sectionsRepository.GetAll().First().SectionID;

            _sectionsService.RemoveSection(sectionID);

            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().BeEmpty();            
        }

        [TestMethod]
        public void SectionsService_RemoveSection_Should_Throw_Exception_When_Have_Less_Than_10_Days_To_Section_Date()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddDays(9), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);
            int sectionID = _sectionsRepository.GetAll().First().SectionID;

            Action act = () =>_sectionsService.RemoveSection(sectionID);

            act.Should().Throw<Exception>().WithMessage("Não é possível remover uma seção se estiver tão proxima de ocorrer.");
            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(1);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
        }

        [TestMethod]
        public void SectionsService_AddSection_Should_Add_Section_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now, Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = new Movie { Tittle = movie1.Tittle};
            section.Room = new Room { Name = room.Name };

            _sectionsService.AddSection(section);

            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(1);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Throw_Exception_When_Another_Section_is_Created_In_Same_Room_With_StartDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddMinutes(10), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Section section2 = new Section { StartDate = DateTime.Now.AddMinutes(100), Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            section2.Movie = new Movie { Tittle = movie2.Tittle };
            section2.Room = new Room { Name = room.Name };

            Action act = () => _sectionsService.AddSection(section2);

            act.Should().Throw<Exception>().WithMessage("Não é possivel criar uma seção no horario que já existe outra seção.");
            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(1);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Throw_Exception_When_Another_Section_is_Created_In_Same_Room_With_EndDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddMinutes(100), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Section section2 = new Section { StartDate = DateTime.Now.AddMinutes(10), Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            section2.Movie = new Movie { Tittle = movie2.Tittle };
            section2.Room = new Room { Name = room.Name };

            Action act = () => _sectionsService.AddSection(section2);

            act.Should().Throw<Exception>().WithMessage("Não é possivel criar uma seção no horario que já existe outra seção.");
            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(1);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Add_Section_When_Another_Section_is_Created_In_Other_Room_With_StartDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddMinutes(10), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Room room2 = new Room { Name = "Sala 9", SeatsNumber = 40 };
            Section section2 = new Section { StartDate = DateTime.Now.AddMinutes(100), Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            new RoomsRepository(_theaterContext).Insert(room2);
            section2.Movie = new Movie { Tittle = movie2.Tittle };
            section2.Room = new Room { Name = room2.Name };

           _sectionsService.AddSection(section2);
           
            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(2);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
            sectionsOnDB.Last().Movie.Tittle.Should().Be(movie2.Tittle);
            sectionsOnDB.Last().Room.Name.Should().Be(room2.Name);
            sectionsOnDB.Last().Value.Should().Be(section2.Value);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Add_Section_When_Another_Section_is_Created_In_Other_Room_With_EndDate_After_Section_Start_And_Before_Section_End()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddMinutes(100), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Room room2 = new Room { Name = "Sala 9", SeatsNumber = 40 };
            Section section2 = new Section { StartDate = DateTime.Now.AddMinutes(10), Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            new RoomsRepository(_theaterContext).Insert(room2);
            section2.Movie = new Movie { Tittle = movie2.Tittle };
            section2.Room = new Room { Name = room2.Name };

            _sectionsService.AddSection(section2);

            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(2);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
            sectionsOnDB.Last().Movie.Tittle.Should().Be(movie2.Tittle);
            sectionsOnDB.Last().Room.Name.Should().Be(room2.Name);
            sectionsOnDB.Last().Value.Should().Be(section2.Value);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Add_Section_When_Another_Section_is_Created_In_Same_Room_With_StartDate_After_Section_End()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddMinutes(140), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Section section2 = new Section { StartDate = DateTime.Now.AddMinutes(10), Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            section2.Movie = new Movie { Tittle = movie2.Tittle };
            section2.Room = new Room { Name = room.Name };

            _sectionsService.AddSection(section2);

            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(2);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
            sectionsOnDB.Last().Movie.Tittle.Should().Be(movie2.Tittle);
            sectionsOnDB.Last().Room.Name.Should().Be(room.Name);
            sectionsOnDB.Last().Value.Should().Be(section2.Value);
        }

        [TestMethod]
        public void SectionsService_GetSections_Should_Add_Section_When_Another_Section_is_Created_In_Same_Room_With_EndDate_Before_Section_Start()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23 };
            Section section = new Section { StartDate = DateTime.Now.AddMinutes(10), Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 120 };
            Section section2 = new Section { StartDate = DateTime.Now.AddMinutes(165), Value = 15, AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed };
            _moviesRepository.Insert(movie2);
            section2.Movie = new Movie { Tittle = movie2.Tittle };
            section2.Room = new Room { Name = room.Name };

            _sectionsService.AddSection(section2);

            IEnumerable<Section> sectionsOnDB = _sectionsRepository.GetAll();
            sectionsOnDB.Should().NotBeEmpty();
            sectionsOnDB.Should().HaveCount(2);
            sectionsOnDB.First().Movie.Tittle.Should().Be(movie1.Tittle);
            sectionsOnDB.First().Room.Name.Should().Be(room.Name);
            sectionsOnDB.First().Value.Should().Be(section.Value);
            sectionsOnDB.Last().Movie.Tittle.Should().Be(movie2.Tittle);
            sectionsOnDB.Last().Room.Name.Should().Be(room.Name);
            sectionsOnDB.Last().Value.Should().Be(section2.Value);
        }
    }
}
