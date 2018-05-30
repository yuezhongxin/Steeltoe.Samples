
using FortuneTeller4.Services;
using FortuneTellerService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;

namespace FortuneTellerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<FortuneContext>(
                options => options.UseInMemoryDatabase("Fortunes"), ServiceLifetime.Singleton);

            services.AddSingleton<IFortuneRepository, FortuneRepository>();

            services.AddDiscoveryClient(Configuration);

            // Register FortuneService Hystrix command
            services.AddHystrixCommand<IEurekaClientService, EurekaClientService>("eureka-client", Configuration);

            // Add Hystrix metrics stream to enable monitoring 
            services.AddHystrixMetricsStream(Configuration);

            // Add framework services.
            services.AddMvc();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            app.UseStaticFiles();

            app.UseMvc();

            // Add Hystrix Metrics context to pipeline
            app.UseHystrixRequestContext();

            app.UseDiscoveryClient();

            // Startup Hystrix metrics stream
            app.UseHystrixMetricsStream();

            SampleData.InitializeFortunesAsync(app.ApplicationServices).Wait();
        }

    }
}
