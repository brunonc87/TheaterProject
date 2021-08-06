using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Application.Movies;
using Theater.Application.Movies.Commands;
using Theater.Application.Movies.Handlers;
using Theater.Application.Movies.Models;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;

namespace Theater.Unit.Tests.Application
{
    [TestClass]
    public class MoviesHandlersTests
    {
        private Mock<IMoviesRepository> _moviesRepository;
        private Mock<ISectionsRepository> _sectionsRepository;
        private IMapper _mapper;

        [TestInitialize]
        public void Initialize()
        {
            _moviesRepository = new Mock<IMoviesRepository>();
            _sectionsRepository = new Mock<ISectionsRepository>();
            _mapper = new MapperConfiguration(mc => mc.AddProfile(new MoviesMappingProfile())).CreateMapper();
        }

        [TestMethod]
        public void GetAllMoviesHandler_Returns_A_List_Of_Movies()
        {
            Movie movie1 = new Movie { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            Movie movie2 = new Movie { Tittle = "filme 2", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Setup(x => x.GetAll()).Returns(new List<Movie> { movie1, movie2 });

            GetAllMoviesHandler getAllMoviesHandler = new GetAllMoviesHandler(_moviesRepository.Object, _mapper);
            IEnumerable<MovieModel> movies = getAllMoviesHandler.Handle(new Theater.Application.Movies.Querys.AllMoviesQuery(), new System.Threading.CancellationToken()).Result;

            movies.Should().NotBeNull();
            movies.Should().HaveCount(2);
            movies.First().Tittle.Should().Be(movie1.Tittle);
            movies.Last().Description.Should().Be(movie2.Description);
        }

        [TestMethod]
        public void GetAllMoviesHandler_Returns_An_Empty_List_Of_Movies_When_No_Movie_Is_On_Database()
        {
            _moviesRepository.Setup(x => x.GetAll());

            GetAllMoviesHandler getAllMoviesHandler = new GetAllMoviesHandler(_moviesRepository.Object, _mapper);
            IEnumerable<MovieModel> moviesOnDB = getAllMoviesHandler.Handle(new Theater.Application.Movies.Querys.AllMoviesQuery(), new System.Threading.CancellationToken()).Result;

            moviesOnDB.Should().NotBeNull();
            moviesOnDB.Should().HaveCount(0);
        }

        [TestMethod]
        public void GetMovieHandler_Returns_Movie_Based_On_Id()
        {
            int movieId = 7;
            Movie movie = new Movie { MovieID = movieId, Tittle = "filme 2", Description = "descrição 2", Duration = 666 };
            _moviesRepository.Setup(x => x.GetByID(It.Is<int>(id => id == movieId))).Returns(movie).Verifiable();

            GetMovieHandler getMovieHandler = new GetMovieHandler(_moviesRepository.Object, _mapper);
            MovieModel moviesOnDB = getMovieHandler.Handle(new Theater.Application.Movies.Querys.MovieQuery(movieId), new System.Threading.CancellationToken()).Result;

            moviesOnDB.Should().NotBeNull();
            moviesOnDB.Id.Should().Be(movie.MovieID);
            moviesOnDB.Tittle.Should().Be(movie.Tittle);
            moviesOnDB.Description.Should().Be(movie.Description);
            moviesOnDB.Duration.Should().Be(movie.Duration);
            _moviesRepository.Verify();
        }

        [TestMethod]
        public void GetMovieHandler_Returns_Null_When_Movie_Is_Not_On_Database()
        {
            int movieId = 7;
            _moviesRepository.Setup(x => x.GetByID(It.Is<int>(id => id == movieId))).Verifiable();

            GetMovieHandler getMovieHandler = new GetMovieHandler(_moviesRepository.Object, _mapper);
            MovieModel moviesOnDB = getMovieHandler.Handle(new Theater.Application.Movies.Querys.MovieQuery(movieId), new System.Threading.CancellationToken()).Result;

            moviesOnDB.Should().BeNull();
            _moviesRepository.Verify();
        }

        [TestMethod]
        public void AddMovieHandler_Should_Return_True_When_Movie_Is_Inserted()
        {
            MovieAddCommand movie1 = new MovieAddCommand { Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movie1.Tittle))).Verifiable();
            _moviesRepository.Setup(x => x.Insert(It.Is<Movie>(m => m.Tittle == movie1.Tittle && m.Description == movie1.Description && m.Duration == movie1.Duration))).Returns(true).Verifiable();

            AddMovieHandler addMovieHandler = new AddMovieHandler(_moviesRepository.Object, _mapper);
            bool result = addMovieHandler.Handle(movie1, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.VerifyAll();
        }

        [TestMethod]
        public void AddMovieHandler_Should_Throw_Exception_When_Movie_With_Same_Tittle_Is_ON_Database()
        {
            Movie movieOnDB = new Movie { MovieID = 1, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            MovieAddCommand movieAddCommand = new MovieAddCommand { Tittle = "filme 1", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movieAddCommand.Tittle))).Returns(movieOnDB).Verifiable();

            AddMovieHandler addMovieHandler = new AddMovieHandler(_moviesRepository.Object, _mapper);
            Action act = () => addMovieHandler.Handle(movieAddCommand, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Já existe um filme com este título cadastrado.");
            _moviesRepository.Verify();
            _moviesRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void UpdateMovieHandler_Should_Update_The_Movie_On_Database()
        {
            Movie movieOnDB = new Movie { MovieID = 10, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            MovieUpdateCommand movieUpdateCommand = new MovieUpdateCommand { ID = 10, Tittle = "filme 2", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Setup(x => x.GetByID(It.Is<int>(m => m == movieUpdateCommand.ID))).Returns(movieOnDB).Verifiable();
            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movieUpdateCommand.Tittle))).Verifiable();
            _moviesRepository.Setup(x => x.Update(It.Is<Movie>(m => m.MovieID == movieUpdateCommand.ID && m.Tittle == movieUpdateCommand.Tittle &&
                                                                    m.Description == movieUpdateCommand.Description && m.Duration == movieUpdateCommand.Duration))).Returns(true).Verifiable();

            UpdateMovieHandler updateMovieHandler = new UpdateMovieHandler(_moviesRepository.Object, _mapper);
            bool result = updateMovieHandler.Handle(movieUpdateCommand, new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateMovieHandler_Should_Throw_Exception_When_MovieID_Not_Found_On_Database()
        {
            MovieUpdateCommand movieUpdateCommand = new MovieUpdateCommand { ID = 10, Tittle = "filme 2", Description = "descrição 2", Duration = 666 };

            _moviesRepository.Setup(x => x.GetByID(It.Is<int>(m => m == movieUpdateCommand.ID))).Verifiable();

            UpdateMovieHandler updateMovieHandler = new UpdateMovieHandler(_moviesRepository.Object, _mapper);
            Action act = () => updateMovieHandler.Handle(movieUpdateCommand, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Filme não encontrado.");
            _moviesRepository.Verify();
            _moviesRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void UpdateMovieHandler_Should_Throw_Exception_When_Update_One_Movie_And_The_Tittle_Is_The_Same_As_Other_Movie_On_Database()
        {
            Movie movieOnDatabase = new Movie { MovieID = 100, Tittle = "filme 1", Description = "descrição 1", Duration = 150 };
            MovieUpdateCommand movieUpdateCommand = new MovieUpdateCommand { ID = 100, Tittle = "filme 3", Description = "descrição 2", Duration = 666 };
            Movie movieTittleOnDatabase = new Movie { MovieID = 200, Tittle = "filme 3", Description = "descrição 3", Duration = 777 };

            _moviesRepository.Setup(x => x.GetByID(It.Is<int>(m => m == movieUpdateCommand.ID))).Returns(movieOnDatabase).Verifiable();
            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movieUpdateCommand.Tittle))).Returns(movieTittleOnDatabase).Verifiable();

            UpdateMovieHandler updateMovieHandler = new UpdateMovieHandler(_moviesRepository.Object, _mapper);
            Action act = () => updateMovieHandler.Handle(movieUpdateCommand, new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Já existe um filme com este título cadastrado.");
            _moviesRepository.Verify();
            _moviesRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteMovieHandler_Should_Remove_Movie_From_Database()
        {
            string movieTittle = "filme 1";
            Movie movieOnDb = new Movie { MovieID = 100, Tittle = movieTittle, Description = "descrição 1", Duration = 150 };

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movieTittle))).Returns(movieOnDb).Verifiable();
            _moviesRepository.Setup(x => x.Delete(It.Is<Movie>(m => m.MovieID == movieOnDb.MovieID && m.Tittle == movieOnDb.Tittle &&
                                                                    m.Description == movieOnDb.Description && m.Duration == movieOnDb.Duration))).Returns(true).Verifiable();
            _sectionsRepository.Setup(x => x.GetByMovieID(It.Is<int>(m => m == movieOnDb.MovieID))).Verifiable();

            DeleteMovieHandler deleteMovieHandler = new DeleteMovieHandler(_moviesRepository.Object, _sectionsRepository.Object);
            bool result = deleteMovieHandler.Handle(new MovieDeleteCommand(movieTittle), new System.Threading.CancellationToken()).Result;

            result.Should().BeTrue();
            _moviesRepository.Verify();
            _sectionsRepository.Verify();
        }

        [TestMethod]
        public void DeleteMovieHandler_Should_Throw_Exception_When_Movie_Not_Found_On_Database()
        {
            string movieTittle = "filme 1";

            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movieTittle)));

            DeleteMovieHandler deleteMovieHandler = new DeleteMovieHandler(_moviesRepository.Object, _sectionsRepository.Object);
            Action act = () => deleteMovieHandler.Handle(new MovieDeleteCommand(movieTittle), new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Filme não encontrado.");
            _moviesRepository.Verify();
            _sectionsRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteMovieHandler_Should_Throw_Exception_When_Movie_Is_In_Any_Section()
        {
            string movieTittle = "filme 1";
            Movie movieOnDb = new Movie { MovieID = 100, Tittle = movieTittle, Description = "descrição 1", Duration = 150 };
            Room roomOnDb = new Room { RoomID = 150, Name = "Sala 7", SeatsNumber = 23 };
            Section sectionOnDb = new Section { SectionID = 300, StartDate = DateTime.Now, Value = 12, AnimationType = AnimationType.D3,
                                                AudioType = AudioType.Original, Movie = movieOnDb, Room = roomOnDb };
            
            
            _moviesRepository.Setup(x => x.GetByTittle(It.Is<string>(m => m == movieTittle))).Returns(movieOnDb).Verifiable();
            _sectionsRepository.Setup(x => x.GetByMovieID(It.Is<int>(m => m == movieOnDb.MovieID))).Returns(new List<Section> { sectionOnDb }).Verifiable();            

            DeleteMovieHandler deleteMovieHandler = new DeleteMovieHandler(_moviesRepository.Object, _sectionsRepository.Object);
            Action act = () => deleteMovieHandler.Handle(new MovieDeleteCommand(movieTittle), new System.Threading.CancellationToken());

            act.Should().Throw<Exception>().WithMessage("Não é possível excluir um filme vinculado a alguma seção.");
            _moviesRepository.Verify();
            _moviesRepository.VerifyNoOtherCalls();
            _sectionsRepository.Verify();
            _sectionsRepository.VerifyNoOtherCalls();
        }
    }
}
