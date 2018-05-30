
using FortuneTeller4.Services;
using FortuneTellerService4.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FortuneTellerService4.Controllers
{
    public class FortunesController : ApiController
    {
        private IFortuneRepository _fortunes;
        private IEurekaClientService _eurekaClientService;
        private ILogger<FortunesController> _logger;

        public FortunesController(IFortuneRepository fortunes, IEurekaClientService eurekaClientService, ILoggerFactory logFactory = null)
        {
            _fortunes = fortunes;
            _eurekaClientService = eurekaClientService;
            _logger = logFactory?.CreateLogger<FortunesController>();
        }

        // GET: api/fortunes
        [HttpGet]
        public IEnumerable<Fortune> Get()
        {
            _logger?.LogInformation("api/fortunes");
            return _fortunes.GetAll();
        }

        // GET: api/services
        [HttpGet]
        public async Task<IHttpActionResult> GetServices()
        {
            _logger?.LogInformation("api/services");
            return Ok(await _eurekaClientService.GetServices());
        }

        // GET: api/services/hystrix
        [HttpGet]
        public async Task<IHttpActionResult> GetServicesWithHystrix()
        {
            _logger?.LogInformation("api/services/hystrix");
            return Ok(await _eurekaClientService.GetServicesWithHystrix());
        }

        // GET api/fortunes/random
        [HttpGet]
        public IHttpActionResult Random()
        {
            _logger?.LogInformation("api/fortunes/random");
            return Ok(_fortunes.RandomFortune());
        }
    }
}
