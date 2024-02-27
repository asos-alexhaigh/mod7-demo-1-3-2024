using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkYourLark.WebApi.Data;
using ParkYourLark.WebApi.StartUp;

namespace ParkYourLark.Tests
{
    public class TestServicesConfiguration : IStartupConfigurationService
    {
        private readonly IDataAccess _dataAccess;

        public TestServicesConfiguration(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public void Configure(IApplicationBuilder app)
        {
        }

        public void ConfigureService(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient(f => _dataAccess);
        }
    }
}