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