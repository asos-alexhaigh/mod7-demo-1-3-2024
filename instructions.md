## Introduction

Give a general explanation of what we're doing:

The approach we'll take in this demo is very similar to what we've talked about in the ATDD talk; we'll take a scenario, and some "given-when-then" criteria; then we'll write a corresponding acceptance test consistent with that criteria.  

As we're writing our acceptance test, we'll get to a certain point with the "outer-loop" coding where our acceptance test will be red.  At that point we'll need to switch to the "inner-loop" and write some unit tests, to drive the implementation; then we'll employ the red-green-refactor process that we've been using in previous modules.

Eventually once all our inner-loop unit tests are passing, our acceptance test should go green, because by making the implementation work in the inner loop, we will have made the outer loop pass automatically.  

In the interest of time we'll need to take certain shortcuts, i.e. copy and paste some code into our solution; don't get too caught up in the implementation details.  What we're hoping to show here is this combination of the outer and inner loop coding, in other words, "ATDD" or "outside-in TDD".

Introduce + show the following concepts/pieces of the code:

1. ASP.NET Core Web API project
2. Entry point of API and Host configuration (_Program.cs_)
3. Startup and Test Startup
4. Testing libraries that are being used (_NUnit, Moq, BDDFy_)

## ATDD Strategies for Given, When, Then

There are different ways to approach writing a test with ATDD.

1. Start with the _Then_ (the assertion), then implement each step of the _When_ and _Given_

2. Start with the _Given_, and implement each step in order, running tests after every step is implemented.

We will be using the latter approach, starting with our GIVEN step that sets up the scenario being tested.

## Writing the test

In the demo, you'll be writing a test for the following requirement, as expressed in a _Given, When, Then_ format:

The scenario, our acceptance criteria as as follows:

```
Scenario
Level is created if it doesn't exist when adding the space
```

```
GIVEN level L1 does not exist in the app
WHEN the administrator adds space S1 on level L1
THEN level L1 exists in the app
AND space S1 on level L1 exists in the app
```

The intention of this demo will be to flesh out the system over time by fulfilling each section of this requirement, in order of the "GWT" steps.

It should be assumed that we have planned out the design of this system before writing the code, using CRC cards or other planning methods.

### Demo Script

First we will be defining our GIVEN step which will set up the mocks required for our scenario.

1. In _LarkIsParked.cs_, add create the test and the Given step:

```csharp
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
        .BDDfy();
}
```

2. Create the Given method 
> (manually or using Resharper; shortcut: `ALT + Enter`)

```csharp
// LarkIsParked.cs
private void LevelL1DoesNotExist()
{
  throw new System.NotImplementedException();
}
```

3. We will now need to create a mock of _IDataAccess_, so that we can arrange the system's behavior as we need for the test:

```csharp
// LarkIsParked.cs
private Mock<IDataAccess> _dataAccessMock;

[SetUp]
public void Setup()
{
  _dataAccessMock = new Mock<IDataAccess>();
}
```

4. Now, we can mock the state of the system for the Given step.

```csharp
// LarkIsParked.cs
private void LevelL1DoesNotExist()
{
    _dataAccessMock.Setup(m => m.Get<Level>()).Returns(new List<Level>());
}
```

5. Create the _Level_ domain object in the `Data` folder of the WebApi project:

```csharp
// Level.cs
public class Level 
{

}
```

6. Create the _Get_ method on the _IDataAccess_ class 
> (manually or using Resharper; shortcut: `ALT + Enter`)

Update the return type from `void` to `IEnumerable<T>` as we know we want the mock to return `new List<Level>()`

```csharp
// IDataAccess.cs
IEnumerable<T> Get<T>();
```

7. Run the test. As we haven't written any assertions yet the current state of the test should pass and be in the green.

8. Add a call to a `When` method to the test for the next step:

```csharp
// LarkIsParked.cs
[Test]
public void LevelIsCreatedIfItDoesNotExist()
{
this
  .Given(_ => LevelL1DoesNotExist())
  .When(_ => TheAdministratorAddsSpaceToLevel("S1", "L1"))
  .BDDfy();
}
```

9. Create the _When_ method 
> (manually or using Resharper; shortcut: `ALT + Enter`)

```csharp
// LarkIsParked.cs
private void TheAdministratorAddsSpaceToLevel(string spaceId, string levelId)
{
  throw new System.NotImplementedException();
}
```

10. We need to implement the action now of calling the API. In order to do this is an efficient and isolated manner, we will host the API within the test process (also know as hosting "in-memory"), and use this for testing purposes. We will create the _ParkYourLarkApi_ class to manage this:

Create a new class called ParkYourLarkApi, in the Test project, then copy in the follow code

```csharp
// ParkYourLarkApi.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using ParkYourLark.WebApi.Data;
using ParkYourLark.WebApi.StartUp;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace ParkYourLark.Tests
{
    public class ParkYourLarkApi
    {
        private readonly Mock<IDataAccess> _dataAccessMock;
        private TestServer _testServer;

        public ParkYourLarkApi(Mock<IDataAccess> dataAccessMock)
        {
            _dataAccessMock = dataAccessMock;

            StartTestHostedApi();
        }

        private void StartTestHostedApi()
        {
            var webHostBuilder = new WebHostBuilder();

            webHostBuilder
                .ConfigureServices(s => s.AddSingleton<IStartupConfigurationService>(f => new TestServicesConfiguration(_dataAccessMock.Object)))
                .UseStartup<Startup>();

            _testServer = new TestServer(webHostBuilder);
        }

        public void AddSpaceToLevel(string space, string level)
        {
            var httpClient = _testServer.CreateClient();

            var requestBody = new
            {
                Space = space,
                Level = level
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/admin/space")
            {
                Content = requestContent
            };

            var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }
    }
}

```

11. Give an overview of how _ParkYourLarkApi.cs_ works.

Note that we have added the line `var request = new HttpRequestMessage(HttpMethod.Post, "api/admin/space")` to target an API controller to manage admin actions. This endpoint doesn't exist yet, but we would have decided as a part of our planning process that it needed to be created.

In a real world scenario you would be referencing a plan you can created ahead of time, using a process such as CRC cards, when creating any component of your design (classes, endpoints, etc.)

12. Add an instance variable of the new ParkYourLarkApi class to _LarkIsParked.cs_, and instantiate this within the test setup. We will then call this in the _When_ step:

```csharp
// LarkIsParked.cs
private Mock<IDataAccess> _dataAccessMock;
private ParkYourLarkApi _parkYourLarkApi;

[SetUp]
public void Setup()
{
    _dataAccessMock = new Mock<IDataAccess>();
    _parkYourLarkApi = new ParkYourLarkApi(_dataAccessMock);
}
```

```csharp
// LarkIsParked.cs
private void TheAdministratorAddsSpaceToLevel(string spaceId, string levelId)
{
    _parkYourLarkApi.AddSpaceToLevel(spaceId, levelId);
}
```

13. Run the test.  It should now be red.  Note that this is not "failing for the right reason", as it's not failing on an assertion; it's failing due to a 404 error during test set up. We can add the _AdminController_ to the production code to make this pass:

```csharp
// AdminController.cs
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{   
    [HttpPost("space")]
    public void Post([FromBody]object request) {}
}
```

14. Run the test. Ensure that it is green, now that we have fixed the `404` error; we have now implemented the _Given_ and _When_ step.

15. Add a method to the test for the first _then_ step:

```csharp
// LarkIsParked.cs
[Test]
public void LevelIsCreatedIfItDoesNotExist()
{
    this
        .Given(_ => LevelL1DoesNotExist())
        .When(_ => TheAdministratorAddsSpaceToLevel("S1", "L1"))
        .Then(_ => LevelL1Exists())
        .BDDfy();
}
```

16. Create the _Then_ method
> (manually or using Resharper; shortcut: `ALT + Enter`)

```csharp
// LarkIsParked.cs
private void LevelL1Exists()
{
    throw new System.NotImplementedException();
}
```

17. Implement the _Then_ step with a verify against the data access mock, as this is our current test boundary:

```csharp
// LarkIsParked.cs
private void LevelL1Exists()
{
    _dataAccessMock.Verify(m => m.Add(It.Is<Level>(level => level.Id == "L1")));
}
```

18. Create the _AddLevel_ method to the _IDataAccess_ interface
> (manually or using Resharper)

```csharp
// IDataAccess.cs
void Add<T>(T entity);
```

19. Add level Id to _Level_ class.
```csharp
// Level.cs
public class Level
{
    public string Id { get; set; }
}
```

20. Run the test. It should now fail on the _Then_ step, for the right reason.
Show the test results and explain why they are "the right reason" i.e. failing because the assertion is false, as opposed to failing due to incorrect set up on the "arrange or act" steps.


21. Add another test step to Acceptance test assertions (shortcut):
```csharp
// LarkIsParked.cs
.And(_ => SpaceS1OnLevelL1Exists())
```

22. Implement SpaceS1OnLevelL1Exists
```csharp
// LarkIsParked.cs
private void SpaceS1OnLevelL1Exists()
{
	_dataAccessMock.Verify(m => m.Add(
    It.Is<LevelSpace>(levelSpace => levelSpace.Level.Id == "L1" && levelSpace.Space == "S1")));
}
```

23. Create _LevelSpace_ class with necessary fields, in the `Data` folder of the webApi project:

```csharp
// LevelSpace.cs
public class LevelSpace
{
    public Level Level { get; set; }
    public string Space { get; set; }
}
```

We have now completed the outer loop of our double loop.
We defined our acceptance criteria, we know what scenarios need to pass for it to be done, so now we are ready to drive the implementation in the codebase using TDD.

We will now start working on the inner loop; that is, we start with a failing acceptance test; now we need to write unit tests and implement the inner workings of our classes, and if we implement them correctly, eventually our outer loop acceptance test should go green.

Why do we start with a red test? (Answer: To avoid false positives)

24. First 'Inner Loop' iteration. We create the AdminControllerShould unit test class in the Tests project.

```csharp
// AdminControllerShould.cs
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
}
```

25. Create a class called RequestParser, and an interface called IRequestParser, in the `Data` folder of the WebApi project.

Mention that normally you'll create _RequestParserShould_ unit tests and then drive it's behaviour from tests. To save time during the demo, we're adding a fully implemented  _RequestParser_. Note we need to write a bit of code to get around how Microsoft handles the `dynamic` parameter.

```csharp
// RequestParser.cs
public class RequestParser : IRequestParser
{
    public LevelSpace Parse(dynamic value)
    {
        JsonNode data = JsonSerializer.Deserialize<JsonNode>(value);
        string levelId = (string)data["Level"];
        string spaceId = (string)data["Space"];

        return new LevelSpace() { Level = new Level { Id = levelId }, Space = spaceId };
    }
}
	
// IRequestParser.cs
public interface IRequestParser
{
    LevelSpace Parse(dynamic value);
}
```

26. Add service registration to _Startup.cs_ for RequestParser:

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(opt => opt.EnableEndpointRouting = false);

    services.AddTransient<IRequestParser, RequestParser>();

    _externalStartupConfigService.ConfigureService(services, null);
}
```

27. We're now able to start our red/green/refactor process in the first inner loop. Update _AdminController_ class to implement a contructor:

```csharp
// AdminController.cs
private readonly IRequestParser _requestParser;
private readonly IDataAccess _dataAccess;

public AdminController(IRequestParser requestParser, IDataAccess dataAccess)
{
    _requestParser = requestParser;
    _dataAccess = dataAccess;
}
```

28. Run your test and "ExtractIdsFromInput" should be red.

29. Implement RequestParser call in the _AdminController_ then re-run your test; it should be green.

```csharp
[HttpPost("space")]
public void Post([FromBody]object request)
{
    _requestParser.Parse(request);
}
```

30. We can now work on a second inner loop test. Add another unit test to _AdminControllerShould_:

```csharp
// AdminControllerShould.cs
[Test]
public void ShouldCreateLevelIfItDoesNotExist()
{
    var mockParser = new Mock<IRequestParser>();
    mockParser.Setup(x => x.Parse(It.IsAny<object>()))
        .Returns(new LevelSpace()
        {
            Level = new Level() { Id = "L1"}
        });

    var dataAccessMock = new Mock<IDataAccess>();

    dataAccessMock.Setup(x => x.Get<Level>()).Returns(new List<Level>());

    var sut = new AdminController(mockParser.Object, dataAccessMock.Object);

    sut.Post("");

    dataAccessMock.Verify(x => x.Add(It.IsAny<Level>()), Times.Once);
}
```

31. Update implementation of Controller to check if level exists and add if not:

```csharp
// AdminController.cs
[HttpPost("space")]
public void Post([FromBody]object request)
{
    var levelSpace = _requestParser.Parse(request);
    var levels = _dataAccess.Get<Level>();

    if (!levels.Contains(levelSpace.Level))
    {
        _dataAccess.Add(levelSpace.Level);
    }
}
```

32. Run the test; at this point it should be green.

33. We can now work on a third inner loop test. Create another unit test for adding LevelSpace:

```csharp
// AdminControllerShould.cs
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

```

34. Implement adding LevelSpace in controller:
```csharp
// AdminController.cs
[HttpPost("space")]
public void Post([FromBody]object request)
{
    var levelSpace = _requestParser.Parse(request);
    var levels = _dataAccess.Get<Level>();

    if (!levels.Contains(levelSpace.Level))
    {
        _dataAccess.Add(levelSpace.Level);
    }
    _dataAccess.Add(levelSpace);
}
```

35. Run all the tests, including the acceptance test that was previously red. It should now be green and passing for the right reason.

Speak about how unit tests play a role in the cycle (and mocking). Point out that the acceptance test is also passing as we have met all the acceptance criteria, by driving the implementation of the inner workings of our applications by doing TDD in the inner loop, which then brings us back to a green test in the outer loop. 