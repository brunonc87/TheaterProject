using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Theater.Domain.Movies;
using Theater.Domain.Rooms;
using Theater.Domain.Sections;

namespace Theater.Unit.Tests.Domain
{
    [TestClass]
    public class SectionTests
    {
        [TestMethod]
        public void Section_CanBeDeleted_Should_Return_True_When_StartDate_Is_More_Than_Ten_Days_Than_DateTimeNow()
        {
            Section section = new Section
            {
                StartDate = DateTime.Now.AddDays(11),
                Movie = new Theater.Domain.Movies.Movie
                {
                    Duration = 120
                }
            };

            section.CanBeDeleted().Should().BeTrue();
        }

        [TestMethod]
        public void Section_CanBeDeleted_Should_Return_False_When_StartDate_Is_Less_Than_Ten_Days_Than_DateTimeNow()
        {
            Section section = new Section
            {
                StartDate = DateTime.Now.AddDays(9),
                Movie = new Theater.Domain.Movies.Movie
                {
                    Duration = 120
                }
            };

            section.CanBeDeleted().Should().BeFalse();
        }

        [TestMethod]
        public void Section_Validate_Should_Not_Throw_Exception_When_Section_Info_Is_OK()
        {
            Section section = new Section { StartDate = DateTime.Now.AddDays(3), AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed, Value = 10, Movie = new Movie(), Room = new Room()};
         
            Action act = () => section.Validate();

            act.Should().NotThrow();
        }

        [TestMethod]
        public void Section_Validate_Should_Throw_Exception_When_Section_StartDate_Is_Lower_Than_DateTime_Now()
        {
            Section section = new Section { StartDate = DateTime.Now.AddDays(-1), AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed, Value = 10, Movie = new Movie(), Room = new Room() };
            
            Action act = () => section.Validate();

            act.Should().Throw<Exception>().WithMessage("A data de inicio não pode ser menor que a data atual");
        }

        [TestMethod]
        public void Section_Validate_Should_Throw_Exception_When_Section_value_Is_Less_Than_1()
        {
            Section section = new Section { StartDate = DateTime.Now.AddDays(1), AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed, Value = 0, Movie = new Movie(), Room = new Room() };

            Action act = () => section.Validate();

            act.Should().Throw<Exception>().WithMessage("Valor do ingresso inválido");
        }

        [TestMethod]
        public void Section_Validate_Should_Throw_Exception_When_Section_value_Is_More_Than_9999()
        {
            Section section = new Section { StartDate = DateTime.Now.AddDays(1), AnimationType = AnimationType.D2, AudioType = AudioType.Dubbed, Value = 10000, Movie = new Movie(), Room = new Room() };

            Action act = () => section.Validate();

            act.Should().Throw<Exception>().WithMessage("Valor do ingresso inválido");
        }

        [TestMethod]
        public void Section_Validate_Should_Throw_Exception_When_AnimationType_Is_Invalid()
        {
            Section section = new Section { StartDate = DateTime.Now.AddDays(1), AnimationType = (AnimationType)3, AudioType = AudioType.Dubbed, Value = 100, Movie = new Movie(), Room = new Room() };

            Action act = () => section.Validate();

            act.Should().Throw<Exception>().WithMessage("Tipo de animação inválido");
        }

        [TestMethod]
        public void Section_Validate_Should_Throw_Exception_When_AudioType_Is_Invalid()
        {
            Section section = new Section { StartDate = DateTime.Now.AddDays(1), AnimationType = AnimationType.D2, AudioType = (AudioType)4, Value = 100, Movie = new Movie(), Room = new Room() };

            Action act = () => section.Validate();

            act.Should().Throw<Exception>().WithMessage("Tipo de audio inválido");
        }
    }
}
