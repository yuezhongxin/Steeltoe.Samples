using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Discovery;
using System.Net.Http;
using System.Threading.Tasks;

namespace FortuneTeller4.Services
{
    public class EurekaClientService : HystrixCommand<string>, IEurekaClientService
    {
        DiscoveryHttpClientHandler _handler;

        private const string GET_SERVICES_URL = "http://eureka-client/home";
        private ILogger<EurekaClientService> _logger;

        public EurekaClientService(IHystrixCommandOptions options, IDiscoveryClient client, ILoggerFactory logFactory = null)
            :base(options)
        {
            _handler = new DiscoveryHttpClientHandler(client, logFactory?.CreateLogger<DiscoveryHttpClientHandler>());
            // Remove comment to use SSL communications with Self-Signed Certs
            // _handler.ServerCertificateCustomValidationCallback = (a,b,c,d) => {return true;};
            IsFallbackUserDefined = true;
            _logger = logFactory?.CreateLogger<EurekaClientService>();
        }

        public async Task<string> GetServices()
        {
            _logger?.LogInformation("GetServices");
            var client = GetClient();
            return await client.GetStringAsync(GET_SERVICES_URL);
 
        }

        public async Task<string> GetServicesWithHystrix()
        {
            _logger?.LogInformation("GetServices");
            var result = await ExecuteAsync();
            _logger?.LogInformation("GetServices returning: " + result);
            return result;
        }

        protected override async Task<string> RunAsync()
        {
            _logger?.LogInformation("RunAsync");
            var client = GetClient();
            var result = await client.GetStringAsync(GET_SERVICES_URL);
            _logger?.LogInformation("RunAsync returning: " + result);
            return result;
        }

        protected override async Task<string> RunFallbackAsync()
        {
            _logger?.LogInformation("RunFallbackAsync");
            return await Task.FromResult("This is a error（服务断开，稍后重试）!");
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
