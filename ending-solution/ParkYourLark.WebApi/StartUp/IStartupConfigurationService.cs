using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParkYourLark.WebApi.StartUp
{
    public interface IStartupConfigurationService
    {
        void Configure(IApplicationBuilder app);

        void ConfigureService(IServiceCollection services, IConfigurationRoot configuration);
    }
}