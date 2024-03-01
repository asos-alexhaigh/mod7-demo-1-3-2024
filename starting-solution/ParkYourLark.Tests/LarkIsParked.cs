using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ParkYourLark.WebApi.Data;
using TestStack.BDDfy;

namespace ParkYourLark.Tests
{
    [Story(
        AsA = "As a lark owner",
        IWant = "I want a parking space for my car",
        SoThat = "So that I can park my lark")]
    internal sealed class LarkIsParked
    {
        // LarkIsParked.cs
        private Mock<IDataAccess> _dataAccessMock;
        private ParkYourLarkApi _parkYourLarkApi;

        [SetUp]
        public void Setup()
        {
            _dataAccessMock = new Mock<IDataAccess>();
            _parkYourLarkApi = new ParkYourLarkApi(_dataAccessMock);
        }

        // LarkIsParked.cs
        [Test]
        public void LevelIsCreatedIfItDoesNotExist()
        {
            /*
                Scenario
                Level is created if it doesn't exist when adding the space

                GIVEN level L1 does not exist in the app
                WHEN the administrator adds space S1 on level L1
                THEN level L1 exists in the app
                AND space S1 on level L1 exists in the app

            */

            this
                .Given(_ => LevelL1DoesNotExist())
                .When(_ => TheAdministratorAddsSpaceToLevel("S1", "L1"))
                .Then(_ => LevelL1Exists())
                .And(_ => SpaceS1OnLevelL1Exists())
                .BDDfy();
        }

        private void LevelL1DoesNotExist()
        {
            _dataAccessMock.Setup(m => m.Get<Level>()).Returns(new List<Level>());
        }

        private void TheAdministratorAddsSpaceToLevel(string spaceId, string levelId)
        {
            _parkYourLarkApi.AddSpaceToLevel(spaceId, levelId);
        }

        private void LevelL1Exists()
        {
            _dataAccessMock.Verify(m => m.Add(It.Is<Level>(level => level.Id == "L1")));
        }

        private void SpaceS1OnLevelL1Exists()
        {
            _dataAccessMock.Verify(m => m.Add(
                It.Is<LevelSpace>(levelSpace => levelSpace.Level.Id == "L1" && levelSpace.Space == "S1")));
        }
    }

    internal class LevelSpace
    {
        public Level Level { get; set; }
        public string Space { get; set; }
    }

    internal class Level
    {
        public string Id { get; set; }
    }
}

/*
 Scenario
   Level is created if it doesn't exist when adding the space

   GIVEN level L1 does not exist in the app
   WHEN the administrator adds space S1 on level L1
   THEN level L1 exists in the app
   AND space S1 on level L1 exists in the app
   AND there are no reservations for space S1 on level L1
*/