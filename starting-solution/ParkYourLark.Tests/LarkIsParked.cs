using TestStack.BDDfy;

namespace ParkYourLark.Tests
{
    [Story(
        AsA = "As a lark owner",
        IWant = "I want a parking space for my car",
        SoThat = "So that I can park my lark")]
    internal sealed class LarkIsParked
    {

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