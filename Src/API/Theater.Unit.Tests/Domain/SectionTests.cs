using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
    }
}
