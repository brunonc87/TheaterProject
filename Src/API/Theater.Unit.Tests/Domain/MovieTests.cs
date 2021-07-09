using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Theater.Domain.Movies;

namespace Theater.Unit.Tests.Domain
{
    [TestClass]
    public class MovieTests
    {
        [TestMethod]
        public void Movie_Validade_Should_Validate_Movie_Properties()
        {
            Movie movie = new Movie { Tittle = "nome", Duration = 34, Description = "descrição" };

            Action act = () => movie.Validate();

            act.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void Movie_Validade_Should_Throw_Exception_When_Tittle_Is_Empty()
        {
            Movie movie = new Movie { Duration = 34, Description = "descrição" };

            Action act = () => movie.Validate();

            act.Should().Throw<Exception>().WithMessage("Título inválido");
        }

        [TestMethod]
        public void Movie_Validade_Should_Throw_Exception_When_Description_Is_Empty()
        {
            Movie movie = new Movie { Tittle = "nome", Duration = 34 };

            Action act = () => movie.Validate();

            act.Should().Throw<Exception>().WithMessage("Descrição inválida");
        }

        [TestMethod]
        public void Movie_Validade_Should_Throw_Exception_When_Duration_Is_Less_Than_1()
        {
            Movie movie = new Movie { Tittle = "nome", Description = "descrição" };

            Action act = () => movie.Validate();

            act.Should().Throw<Exception>().WithMessage("Tempo do filme inválido");
        }

        [TestMethod]
        public void Movie_CopyFrom_Should_Copy_Movie_Data_To_Principal_Movie()
        {
            Movie movie = new Movie { Tittle = "nome", Description = "descrição" };

            Movie movie2 = new Movie();
            movie2.CopyFrom(movie);

            movie2.Tittle.Should().Be(movie.Tittle);
            movie2.Description.Should().Be(movie.Description);
            movie2.Duration.Should().Be(movie.Duration);
        }
    }
}
