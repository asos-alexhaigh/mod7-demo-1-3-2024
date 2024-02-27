using Moq;
using NUnit.Framework;
using ParkYourLark.WebApi.Controllers;
using ParkYourLark.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkYourLark.WebApi;

namespace ParkYourLark.Tests
{
    [TestFixture]
    public class AdminControllerShould
    {

        [Test]
        public void ExtractIdsFromInput()
        {
            var mockParser = new Mock<IRequestParser>();
            mockParser.Setup(x => x.Parse(It.IsAny<object>()))
                .Returns(new LevelSpace()
                {
                    Level = new Level() { Id = "L1" }
                });

            var dataAccessMock = new Mock<IDataAccess>();

            var sut = new AdminController(mockParser.Object, dataAccessMock.Object);

            var value = new
            {
                Space = "S1",
                Level = "L1"
            };

            sut.Post(value);

            mockParser.Verify(parser => parser.Parse(value), Times.Once);
        }

        [Test]
        public void ShouldCreateLevelIfItDoesNotExist()
        {
            var mockParser = new Mock<IRequestParser>();
            mockParser.Setup(x => x.Parse(It.IsAny<object>()))
                .Returns(new LevelSpace()
                {
                    Level = new Level() { Id = "L1" }
                });

            var dataAccessMock = new Mock<IDataAccess>();

            dataAccessMock.Setup(x => x.Get<Level>()).Returns(new List<Level>());

            var sut = new AdminController(mockParser.Object, dataAccessMock.Object);

            sut.Post("");

            dataAccessMock.Verify(x => x.Add(It.IsAny<Level>()), Times.Once);
        }

        [Test]
        public void ShouldCreateSpaceLevel()
        {
            var mockParser = new Mock<IRequestParser>();
            mockParser.Setup(x => x.Parse(It.IsAny<object>()))
                .Returns(new LevelSpace()
                {
                    Level = new Level() { Id = "L1" },
                    Space = "S1"
                });

            var dataAccessMock = new Mock<IDataAccess>();

            dataAccessMock.Setup(x => x.Get<Level>()).Returns(new List<Level>());

            var sut = new AdminController(mockParser.Object, dataAccessMock.Object);

            sut.Post("");

            dataAccessMock.Verify(x => x.Add(It.IsAny<LevelSpace>()), Times.Once);
        }
    }
}
