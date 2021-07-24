using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Application.Movies;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;
using Theater.Infra.Data.Repositories;
using Theater.Integration.Tests.Common;

namespace Theater.Integration.Tests.Application
{
    [TestClass]
    public class MoviesServiceTests : TheaterIntegrationBase
    {
        private IMoviesService _moviesService;
        private IMoviesRepository _moviesRepository;
        private ISectionsRepository _sectionsRepository;

        [TestInitialize]
        public void Initialize()
        {
            base.Reset();

            _moviesRepository = new MoviesRepository(_theaterContext);
            _sectionsRepository = new SectionsRepository(_theaterContext);
            _moviesService = new MoviesService(_moviesRepository, _sectionsRepository);
        }

        [TestMethod]
        public void MoviesService_GetMovies_Returns_A_List_Of_Movies()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 666 };
            _moviesRepository.Insert(movie1);
            _moviesRepository.Insert(movie2);

            IEnumerable<Movie> moviesOnDB = _moviesService.GetMovies();

            moviesOnDB.Should().NotBeNull();
            moviesOnDB.Should().HaveCount(2);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.Last().Description.Should().Be(movie2.Description);
        }

        [TestMethod]
        public void MoviesService_GetMovies_Returns_An_Empty_List_Of_Movies_When_No_Movie_Is_On_Database()
        {
            IEnumerable<Movie> moviesOnDB = _moviesService.GetMovies();

            moviesOnDB.Should().NotBeNull();
            moviesOnDB.Should().HaveCount(0);
        }

        [TestMethod]
        public void MoviesService_GetById_Returns_Movie_Based_On_Id()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 666 };
            _moviesRepository.Insert(movie1);
            _moviesRepository.Insert(movie2);

            Movie moviesOnDB = _moviesService.GetById(2);

            moviesOnDB.Should().NotBeNull();
            moviesOnDB.Tittle.Should().Be(movie2.Tittle);
            moviesOnDB.Description.Should().Be(movie2.Description);
            moviesOnDB.Duration.Should().Be(movie2.Duration);
        }        

        [TestMethod]
        public void MoviesService_GetById_Returns_Null_When_Movie_Is_Not_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            _moviesRepository.Insert(movie1);

            Movie moviesOnDB = _moviesService.GetById(666);

            moviesOnDB.Should().BeNull();
        }

        [TestMethod]
        public void MoviesService_AddMovie_Should_Insert_The_Movie_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };

            _moviesService.AddMovie(movie1);

            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();

            moviesOnDB.Should().NotBeNull();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
        }

        [TestMethod]
        public void MoviesService_AddMovie_Should_Throw_Exception_When_Any_Property_Is_Empty()
        {
            Movie movie1 = new Movie { Tittle = "filme 1",  Duration = 150 };

            Action act = () =>_moviesService.AddMovie(movie1);

            act.Should().Throw<Exception>();
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().BeEmpty();            
        }

        [TestMethod]
        public void MoviesService_AddMovie_Should_Throw_Exception_When_Movie_With_Same_Tittle_Is_ON_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 1", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Insert(movie1);

            Action act = () => _moviesService.AddMovie(movie2);

            act.Should().Throw<Exception>().WithMessage("Já existe um filme com este título cadastrado.");
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
        }

        [TestMethod]
        public void MoviesService_UpdateMovie_Should_Update_The_Movie_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 1", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Insert(movie1);
            movie2.MovieID = _moviesRepository.GetAll().First().MovieID;

            _moviesService.UpdateMovie(movie2);
            
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie2.Description);
            moviesOnDB.First().Duration.Should().Be(movie2.Duration);
        }

        [TestMethod]
        public void MoviesService_UpdateMovie_Should_Throw_Exception_When_MovieID_Not_Found_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 1", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Insert(movie1);
            movie2.MovieID = 9999;

            Action act = () => _moviesService.UpdateMovie(movie2);

            act.Should().Throw<Exception>().WithMessage("Filme não encontrado.");
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
        }

        [TestMethod]
        public void MoviesService_UpdateMovie_Should_Throw_Exception_When_Update_One_Movie_And_The_Tittle_Is_The_Same_As_Other_Movie_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 3", Description = "descrição 2", Duration = 666 };
            Movie movie3 = new Movie { Tittle = "filme 3", Description = "descrição 3", Duration = 777 };

            _moviesRepository.Insert(movie1);
            _moviesRepository.Insert(movie3);
            movie2.MovieID = _moviesRepository.GetAll().First().MovieID;

            Action act = () => _moviesService.UpdateMovie(movie2);

            act.Should().Throw<Exception>().WithMessage("Já existe um filme com este título cadastrado.");
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(2);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
            moviesOnDB.Last().Tittle.Should().Be(movie3.Tittle);
            moviesOnDB.Last().Description.Should().Be(movie3.Description);
            moviesOnDB.Last().Duration.Should().Be(movie3.Duration);
        }

        [TestMethod]
        public void MoviesService_UpdateMovie_Should_Throw_Exception_When_Movie_To_Update_Has_Any_Property_Wrong()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 2", Description = "", Duration = 666 };

            _moviesRepository.Insert(movie1);
            movie2.MovieID = _moviesRepository.GetAll().First().MovieID;

            Action act = () => _moviesService.UpdateMovie(movie2);

            act.Should().Throw<Exception>();
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
        }

        [TestMethod]
        public void MoviesService_DeleteMovie_Should_Remove_Movie_From_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };            

            _moviesRepository.Insert(movie1);

            _moviesService.DeleteMovie(movie1.Tittle);
            
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().BeEmpty();
        }

        [TestMethod]
        public void MoviesService_DeleteMovie_Should_Throw_Exception_When_Movie_Not_Found_On_Database()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };

            _moviesRepository.Insert(movie1);

            Action act = () =>_moviesService.DeleteMovie("filme 2");

            act.Should().Throw<Exception>().WithMessage("Filme não encontrado.");
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
        }

        [TestMethod]
        public void MoviesService_DeleteMovie_Should_Throw_Exception_When_Movie_Is_In_Any_Section()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Room room = new Room { Name = "Sala 7", SeatsNumber = 23};
            Section section = new Section { StartDate = DateTime.Now, Value = 12, AnimationType = AnimationType.D3, AudioType = AudioType.Original };
            _moviesRepository.Insert(movie1);
            new RoomsRepository(_theaterContext).Insert(room);
            section.Movie = _moviesRepository.GetAll().First();
            section.Room = new RoomsRepository(_theaterContext).GetAll().First();
            _sectionsRepository.Insert(section);

            Action act = () => _moviesService.DeleteMovie(movie1.Tittle);

            act.Should().Throw<Exception>().WithMessage("Não é possível excluir um filme vinculado a alguma seção.");
            IEnumerable<Movie> moviesOnDB = _moviesRepository.GetAll();
            moviesOnDB.Should().NotBeEmpty();
            moviesOnDB.Should().HaveCount(1);
            moviesOnDB.First().Tittle.Should().Be(movie1.Tittle);
            moviesOnDB.First().Description.Should().Be(movie1.Description);
            moviesOnDB.First().Duration.Should().Be(movie1.Duration);
        }
    }
}
