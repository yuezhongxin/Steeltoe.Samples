using Autofac;
using Autofac.Integration.WebApi;
using FortuneTeller4.Services;
using FortuneTellerService4.Models;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Logging.Autofac;
using Steeltoe.Common.Options.Autofac;
using System.Reflection;
using System.Web.Http;

namespace FortuneTellerService4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
       
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ApplicationConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Add Microsoft Options to container
            builder.RegisterOptions();

            // Add Microsoft Logging to container
            builder.RegisterLogging(ApplicationConfig.Configuration);

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ApplicationConfig.Configuration);

            // Initialize and Register FortuneContext
            builder.RegisterInstance(SampleData.InitializeFortunes()).SingleInstance();

            // Register FortuneRepository
            builder.RegisterType<FortuneRepository>().As<IFortuneRepository>().SingleInstance();
            //builder.RegisterType<EurekaClientService>().As<IEurekaClientService>().SingleInstance();

            // Register FortuneService Hystrix command
            builder.RegisterHystrixCommand<IEurekaClientService, EurekaClientService > ("eureka-client", ApplicationConfig.Configuration);

            // Register Hystrix Metrics/Monitoring stream
            //builder.RegisterHystrixMetricsStream(ApplicationConfig.Configuration);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Get a logger from container
            var logger = container.Resolve<ILogger<WebApiApplication>>();

            logger.LogInformation("Finished container build, starting background services");

            // Start the Discovery client background thread
            container.StartDiscoveryClient();

            // Start the Hystrix Metrics stream 
            //container.StartHystrixMetricsStream();

            logger.LogInformation("Finished starting background services");

        }

        protected void Application_End()
        {
            // Unregister current app with Service Discovery server
            //_client.ShutdownAsync();
        }
    }
}
