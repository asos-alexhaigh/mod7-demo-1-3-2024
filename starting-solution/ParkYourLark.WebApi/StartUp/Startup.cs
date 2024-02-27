using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParkYourLark.WebApi.StartUp
{
    public class Startup
    {
        private readonly IStartupConfigurationService _externalStartupConfigService;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IStartupConfigurationService externalStartupConfigService)
        {
            _externalStartupConfigService = externalStartupConfigService;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => opt.EnableEndpointRouting = false);

            _externalStartupConfigService.ConfigureService(services, null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _externalStartupConfigService.Configure(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
