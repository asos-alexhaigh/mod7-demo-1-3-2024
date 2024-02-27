using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParkYourLark.WebApi.StartUp
{
    using Data;

    public class ConfigureDataServices : IStartupConfigurationService
    {
        public virtual void Configure(IApplicationBuilder app) { }

        public virtual void ConfigureService(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<IDataAccess, SqlDataAccess>();
        }
    }
}